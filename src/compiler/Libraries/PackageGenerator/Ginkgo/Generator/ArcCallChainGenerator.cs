using System.Diagnostics;
using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

public static class ArcCallChainGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ref ArcFunctionGenerationContext ctx,
        ArcCallChain syntax)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        
        if (syntax.ConstructorCall is not null)
        {
            // TODO: Support constructor call
            throw new NotImplementedException();
        }
        
        // TODO: Support enum member resolution as an exceptional case

        if (!syntax.Terms.Any())
        {
            return result;
        }
        
        var firstTerm = GenerateFirstTerm(ref ctx, syntax.Terms.ElementAt(0));
        if (firstTerm.DiscardResult)
        {
            return result;
        }
        result.Append(firstTerm);

        var prevTermOutReg = firstTerm.OutImplicitRegister;
        foreach (var term in syntax.Terms.Skip(1))
        {
            var termResult = GenerateNextTerm(ref ctx, term, prevTermOutReg);
            if(termResult.DiscardResult)
            {
                return result;
            }
            
            result.Append(termResult);
            prevTermOutReg = termResult.OutImplicitRegister;
        }
        
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
        switch (syntax.Type)
        {
            case ArcCallChainTermType.Identifier:
            {
                var ident = syntax.Identifier!;
                var typeNode = ArcDataTypeHelper.GetDataTypeNode(ctx, baseObj.Instance.Type.ResolvedType)!;

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