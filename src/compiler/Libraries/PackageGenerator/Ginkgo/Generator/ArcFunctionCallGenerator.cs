using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Logging;
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

    public static ArcGinkgoInstructionGenerationResult GenerateSelfCall(
        ref ArcFunctionGenerationContext ctx, ArcFunctionCall syntax, ArcImplicitRegister baseObj)
    {
        var group = ArcDataTypeHelper.GetDataTypeNode(ctx, baseObj.Instance.Type.ResolvedType)?.ComplexTypeGroup;
        return ResolveAndEmit(ref ctx, syntax,
            group?.Functions.Where(f => f.Name == syntax.Identifier.Name).Cast<ArcScopeTreeFunctionNodeBase>() ?? [],
            baseObj);
    }

    private static ArcGinkgoInstructionGenerationResult ResolveAndEmit(
        ref ArcFunctionGenerationContext ctx, ArcFunctionCall syntax,
        IEnumerable<ArcScopeTreeFunctionNodeBase> candidates, ArcImplicitRegister? receiver)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        if (syntax.Context.arc_generic_specialization_wrapper() is not null || syntax.SpecializedGenericTypes.Any())
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Ginkgo does not support generic function specialization", ctx.UnitName, syntax.Identifier);
        var arguments = new List<ArcImplicitRegister>();
        if (receiver is not null) arguments.Add(receiver);
        // Evaluate before preparing the outer frame, so nested calls cannot replace it.
        foreach (var argument in syntax.Arguments)
        {
            var value = ArcExpressionEvaluationGenerator.Generate(ref ctx, argument.Expression);
            result.Append(value);
            if (value.DiscardResult || value.OutImplicitRegister is null) return result;
            arguments.Add(value.OutImplicitRegister);
        }
        var matches = candidates.Where(f => f.Parameters.Count() == arguments.Count &&
            f.Parameters.Zip(arguments).All(pair =>
                pair.First.DataType.Type.TypeId == pair.Second.Instance.Type.ResolvedType.TypeId &&
                pair.First.DataType.Dimension == pair.Second.Instance.Dimension)).ToList();
        if (matches.Count != 1)
        {
            result.Append(ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                matches.Count == 0 ? $"No matching function {syntax.Identifier}" : $"Ambiguous function {syntax.Identifier}",
                ctx.UnitName, syntax.Identifier));
            return result;
        }
        var function = matches[0];
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
        result.Instructions.AddRange(arguments.Select(a => new ArcControlFlowBindArgInstruction { Src = a.Id }));
        var invoke = ArcControlFlowInvokeInstruction.Normal;
        invoke.Dest = output?.Id ?? 0;
        result.Instructions.Add(invoke);
        result.OutImplicitRegister = output;
        result.OutputInvoke = output is null ? null : invoke;
        return result;
    }
}
