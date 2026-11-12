using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

public static class ArcFunctionCallGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ref ArcFunctionGenerationContext ctx, ArcFunctionCall syntax)
    {
        var candidates = syntax.Identifier.Namespace is not null
            ? ctx.GlobalContext.ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>(syntax.Identifier.NameArray)
            : ctx.GlobalContext.LinkedNodes.Concat(ctx.CurrentNode.Namespace?.Children ?? [])
                .OfType<ArcScopeTreeFunctionNodeBase>().Where(f => f.Name == syntax.Identifier.Name).Distinct();
        return ResolveAndEmit(ref ctx, syntax, candidates, null);
    }

    public static ArcGinkgoInstructionGenerationResult GenerateMemberCall(
        ref ArcFunctionGenerationContext ctx, ArcFunctionCall syntax, ArcImplicitRegister baseObj)
    {
        var group = ArcDataTypeHelper.GetDataTypeNode(ctx, baseObj.Instance.Type.ResolvedType)?.ComplexTypeGroup;
        return ResolveAndEmit(ref ctx, syntax,
            group?.Functions.Where(f => f.Name == syntax.Identifier.Name && f.Receiver is not null)
                .Cast<ArcScopeTreeFunctionNodeBase>() ?? [],
            baseObj);
    }

    internal static bool Matches(ArcScopeTreeFunctionNodeBase function, ArcImplicitRegister? receiver,
        IEnumerable<ArcImplicitRegister> arguments)
    {
        if ((function.Receiver is null) != (receiver is null))
            return false;
        if (function.Receiver is not null && !MatchesParameter(function.Receiver, receiver!))
            return false;
        var values = arguments.ToArray();
        return function.ExplicitParameters.Count() == values.Length &&
               function.ExplicitParameters.Zip(values).All(pair => MatchesParameter(pair.First, pair.Second));
    }

    private static bool MatchesParameter(ArcParameterDescriptor parameter, ArcImplicitRegister argument)
        => parameter.DataType.Type.TypeId == argument.Instance.Type.ResolvedType.TypeId &&
           parameter.DataType.Dimension == argument.Instance.Dimension;

    private static ArcGinkgoInstructionGenerationResult ResolveAndEmit(
        ref ArcFunctionGenerationContext ctx, ArcFunctionCall syntax,
        IEnumerable<ArcScopeTreeFunctionNodeBase> candidates, ArcImplicitRegister? receiver)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        if (syntax.Context.arc_generic_specialization_wrapper() is not null || syntax.SpecializedGenericTypes.Any())
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Ginkgo does not support generic function specialization", ctx.UnitName, syntax.Identifier);
        var arguments = new List<ArcImplicitRegister>();
        // Evaluate before preparing the outer frame, so nested calls cannot replace it.
        foreach (var argument in syntax.Arguments)
        {
            var value = ArcExpressionEvaluationGenerator.Generate(ref ctx, argument.Expression);
            result.AppendInstructions(value);
            if (value.DiscardResult || value.OutImplicitRegister is null) return result;
            arguments.Add(value.OutImplicitRegister);
        }
        // Qualified instance calls supply their receiver as the first source argument.
        var matches = candidates.Where(f => receiver is not null
            ? Matches(f, receiver, arguments)
            : f.Receiver is not null
                ? syntax.Identifier.Namespace is not null && arguments.Count > 0 &&
                  Matches(f, arguments[0], arguments.Skip(1))
                : Matches(f, null, arguments)).ToList();
        if (matches.Count != 1)
        {
            result.AppendInstructions(ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                matches.Count == 0 ? $"No matching function {syntax.Identifier}" : $"Ambiguous function {syntax.Identifier}",
                ctx.UnitName, syntax.Identifier));
            return result;
        }
        var function = matches[0];
        if (function.Receiver is not null && receiver is null)
        {
            receiver = arguments[0];
            arguments.RemoveAt(0);
        }
        ArcImplicitRegister? output = null;
        if (function.ReturnValueType.Type.TypeId != 0 || function.ReturnValueType.Dimension != 0)
        {
            var (register, logs) = ArcImplicitRegister.CreateAnonymous(ctx,
                function.ReturnValueType, ArcImplicitRegisterType.LocalData);
            result.Logs.AddRange(logs);
            if (register is null || result.DiscardResult) return result;
            ctx.AddImplicitRegister(register);
            output = register;
        }
        result.Instructions.Add(new ArcControlFlowPrepareCallInstruction(function.Id));
        if (receiver is not null)
            result.Instructions.Add(new ArcControlFlowBindArgInstruction { Src = receiver.Id });
        result.Instructions.AddRange(arguments.Select(a => new ArcControlFlowBindArgInstruction { Src = a.Id }));
        var invoke = ArcControlFlowInvokeInstruction.Normal;
        invoke.Dest = output?.Id ?? 0;
        result.Instructions.Add(invoke);
        result.SetOutput(output, invoke);
        return result;
    }
}
