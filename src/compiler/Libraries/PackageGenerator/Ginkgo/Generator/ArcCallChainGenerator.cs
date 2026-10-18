using System.Diagnostics;
using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

public static class ArcCallChainGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ref ArcFunctionGenerationContext ctx,
        ArcCallChain syntax)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        if (syntax.Terms.Any(t => t.Indices.Any()))
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Ginkgo has no indexed load/store instruction", ctx.UnitName);
        
        if (syntax.ConstructorCall is null && !syntax.Terms.Any())
        {
            return result;
        }
        
        var firstTerm = syntax.ConstructorCall is not null
            ? GenerateConstructor(ref ctx, syntax.ConstructorCall)
            : GenerateFirstTerm(ref ctx, syntax.Terms.ElementAt(0));
        if (firstTerm.DiscardResult)
        {
            return firstTerm;
        }
        result.Append(firstTerm);

        var prevTermOutReg = firstTerm.OutImplicitRegister;
        var outputInvoke = firstTerm.OutputInvoke;
        foreach (var term in syntax.Terms.Skip(syntax.ConstructorCall is null ? 1 : 0))
        {
            var termResult = GenerateNextTerm(ref ctx, term, prevTermOutReg);
            result.Append(termResult);
            if(termResult.DiscardResult)
            {
                return result;
            }
            
            prevTermOutReg = termResult.OutImplicitRegister;
            outputInvoke = termResult.OutputInvoke;
        }
        
        result.OutImplicitRegister = prevTermOutReg;
        result.OutputInvoke = outputInvoke;
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult GenerateConstructor(
        ref ArcFunctionGenerationContext ctx, ArcConstructorCall syntax)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        if (syntax.Context.arc_generic_specialization_wrapper() is not null || syntax.SpecializedGenericTypes.Any())
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Ginkgo does not support generic constructor specialization", ctx.UnitName, syntax.Context);
        }
        
        var type = ArcDataTypeHelper.GetComplexType(ctx, syntax.DataType);
        var group = type is null ? null : ArcDataTypeHelper.GetDataTypeNode(ctx, type.ResolvedType)?.ComplexTypeGroup;
        if (group is null)
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Constructor requires a group type", ctx.UnitName, syntax.Context);
        }
        
        var (output, logs) = ArcImplicitRegister.CreateAnonymous(ctx,
            new ArcDataTypeInstance { Type = type!, Mutability = ArcMutability.Variable }, ArcImplicitRegisterType.LocalData);
        result.Logs.AddRange(logs);
        
        if (output is null)
        {
            return result;
        }
        
        ctx.AddImplicitRegister(output);
        var arguments = new List<ArcImplicitRegister> { output };
        foreach (var parameter in syntax.Parameters)
        {
            var argument = ArcExpressionEvaluationGenerator.Generate(ref ctx, parameter);
            result.Append(argument);
            if (result.DiscardResult || argument.OutImplicitRegister is null)
            {
                return result;
            }
            arguments.Add(argument.OutImplicitRegister);
        }
        
        var constructors = group.LifecycleFunctions.Where(f =>
            f.SyntaxTree.LifecycleStage == ArcGroupLifecycleStageType.Construction).ToList();
        var matches = constructors.Where(f => f.Parameters.Count() == arguments.Count &&
            f.Parameters.Zip(arguments).All(pair =>
                pair.First.DataType.Type.TypeId == pair.Second.Instance.Type.ResolvedType.TypeId &&
                pair.First.DataType.Dimension == pair.Second.Instance.Dimension)).ToList();
        
        // A group with no declared constructors has implicit default construction.
        if (matches.Count != 1 && !(constructors.Count == 0 && arguments.Count == 1))
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                matches.Count > 1 ? "Ambiguous constructor" : "No matching constructor", ctx.UnitName, syntax.Context);
        }
        
        if (matches.Count == 1 && (!matches[0].IsSelfFunction ||
            matches[0].ReturnValueType.Type.TypeId != 0 || matches[0].ReturnValueType.Dimension != 0))
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Ginkgo construction lifecycle functions require an explicit self parameter and a scalar none return type",
                ctx.UnitName, syntax.Context);
        }
        
        result.Instructions.Add(new ArcDataInitObjectInstruction
            { TypeId = checked((uint)type!.ActualTypeId), Dest = output.Id });
        if (matches.Count == 1)
        {
            result.Instructions.Add(new ArcControlFlowPrepareCallInstruction(matches.First().Id));
            result.Instructions.AddRange(arguments.Select(a => new ArcControlFlowBindArgInstruction { Src = a.Id }));
            result.Instructions.Add(ArcControlFlowInvokeInstruction.Normal);
        }
        
        result.OutImplicitRegister = output;
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult GenerateFirstTerm(
        ref ArcFunctionGenerationContext ctx,
        ArcCallChainTerm syntax)
    {
        switch (syntax.Type)
        {
            case ArcCallChainTermType.Identifier:
            {
                var ident = syntax.Identifier!;
                if (ident.Namespace is null)
                {
                    // Check if it is a variable
                    var name = ident.Name;
                    var localObj = ctx.GetImplicitRegister(name);
                    if (localObj is not null)
                    {
                        return new ArcGinkgoInstructionGenerationResult
                        {
                            OutImplicitRegister = localObj,
                        };
                    }
                }

                var err = new ArcGinkgoInstructionGenerationResult();
                err.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"No matching variable candidate found for \"{ident}\"", ctx.UnitName, ident));
                return err;
            }
            case ArcCallChainTermType.FunctionCall:
            {
                var fc = syntax.FunctionCall!;
                var fcRes = ArcFunctionCallGenerator.Generate(ref ctx, fc);
                return fcRes;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static ArcGinkgoInstructionGenerationResult GenerateNextTerm(
        ref ArcFunctionGenerationContext ctx,
        ArcCallChainTerm syntax,
        ArcImplicitRegister baseObj)
    {
        if (baseObj is null)
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Call chain receiver does not produce a value", ctx.UnitName, syntax);
        switch (syntax.Type)
        {
            case ArcCallChainTermType.Identifier:
            {
                var ident = syntax.Identifier!;
                var typeNode = ArcDataTypeHelper.GetDataTypeNode(ctx, baseObj.Instance.Type.ResolvedType);
                if (typeNode is null || typeNode.ComplexTypeGroup is null)
                    return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                        "Receiver has no group fields", ctx.UnitName, ident);

                if (typeNode.ArcDataTypeType == ArcDataTypeType.Primitive)
                {
                    return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                        $"Primitive types do not have methods to invoke", ctx.UnitName, ident);
                }

                if (typeNode.ArcDataTypeType == ArcDataTypeType.Enum)
                {
                    throw new UnreachableException();
                }

                var groupNode = typeNode.ComplexTypeGroup!;
                
                // Find target field
                var field = groupNode.Fields.FirstOrDefault(f => f.IdentifierName == ident.Name);
                if (field is null)
                {
                    return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                        $"Field \"{ident}\" not found in type \"{typeNode.Name}\"", ctx.UnitName, ident);
                }

                var (outObj, outLogs) = ArcImplicitRegister.CreateAnonymous(ctx, field.DataType, ArcImplicitRegisterType.LocalData);

                if (outObj is null)
                {
                    return ArcGinkgoInstructionGenerationResult.WithLogs(outLogs);
                }
                
                ctx.AddImplicitRegister(outObj);
                var loadField = new ArcDataLoadFieldInstruction
                {
                    Src = baseObj.Id,
                    Dest = outObj.Id,
                    FieldId = field.Id,
                };

                var result = new ArcGinkgoInstructionGenerationResult
                {
                    OutImplicitRegister = outObj,
                    Instructions = [loadField]
                };
                
                return result;
            }
            case ArcCallChainTermType.FunctionCall:
            {
                var fc = syntax.FunctionCall!;
                return ArcFunctionCallGenerator.GenerateSelfCall(ref ctx, fc, baseObj);
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
