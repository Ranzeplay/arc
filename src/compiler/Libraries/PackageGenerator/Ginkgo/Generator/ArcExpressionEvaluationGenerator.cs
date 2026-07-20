using System.Diagnostics;
using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Microsoft.Extensions.Logging;
using ArcComparisonLessThanInstruction = Arc.Compiler.PackageGenerator.Ginkgo.Instruction.ArcComparisonLessThanInstruction;
using ArcLogicalAndInstruction = Arc.Compiler.PackageGenerator.Ginkgo.Instruction.ArcLogicalAndInstruction;
using ArcLogicalNotInstruction = Arc.Compiler.PackageGenerator.Ginkgo.Instruction.ArcLogicalNotInstruction;
using ArcLogicalOrInstruction = Arc.Compiler.PackageGenerator.Ginkgo.Instruction.ArcLogicalOrInstruction;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Generator;

public static class ArcExpressionEvaluationGenerator
{
    public static ArcGinkgoInstructionGenerationResult Generate(
        ref ArcFunctionGenerationContext ctx,
        ArcExpression syntax,
        ArcDataDeclarator? topLevelTarget = null)
    {
        if (syntax.DataValue is not null)
        {
            return GenerateDataValue(ref ctx, syntax.DataValue, topLevelTarget);
        }

        if (syntax.Operator is not null)
        {
            return GenerateOperation(ref ctx, syntax, topLevelTarget);
        }

        if (syntax.IsWrapped)
        {
            return Generate(ref ctx, syntax.SubExpressions.ElementAt(0), topLevelTarget);
        }

        throw new UnreachableException();
    }

    private static ArcGinkgoInstructionGenerationResult GenerateDataValue(
        ref ArcFunctionGenerationContext ctx,
        ArcDataValue syntax,
        ArcDataDeclarator? topLevelTarget = null)
    {
        // TODO: implement
        return new();
    }

    private static ArcGinkgoInstructionGenerationResult GenerateOperation(
        ref ArcFunctionGenerationContext ctx,
        ArcExpression syntax,
        ArcDataDeclarator? topLevelTarget = null)
    {
        var result = new ArcGinkgoInstructionGenerationResult();

        if (syntax.Operator is null)
        {
            result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Critical, 0, "Unreachable compiler branch in operation expression", ctx.UnitName, syntax.Context));
            return result;
        }

        if (syntax.Operator.Value.IsBinary())
        {
            return GenerateBinaryOperation(ref ctx, syntax, topLevelTarget);
        }

        if (syntax.Operator.Value.IsUnary())
        {
            return GenerateUnaryOperation(ref ctx, syntax, topLevelTarget);
        }
        
        result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Critical, 0, "Unreachable compiler branch in operation expression", ctx.UnitName, syntax.Context));
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult GenerateBinaryOperation(
        ref ArcFunctionGenerationContext ctx,
        ArcExpression syntax,
        ArcDataDeclarator? topLevelTarget = null)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        var lhs = syntax.SubExpressions.ElementAt(0);
        var lhsResult = Generate(ref ctx, lhs);
        result.Append(lhsResult);
        
        
        var rhs = syntax.SubExpressions.ElementAt(1);
        var rhsResult = Generate(ref ctx, rhs);
        result.Append(rhsResult);
        
        var (outObj, outLogs) = topLevelTarget is null
            ? ArcImplicitRegister.CreateAnonymous(ctx, lhsResult.OutImplicitRegister.Instance, ArcImplicitRegisterType.LocalData)
            : ArcImplicitRegister.Create(ctx, topLevelTarget, ArcImplicitRegisterType.LocalData);

        if (outObj is null)
        {
            result.Logs.AddRange(outLogs);
            return result;
        }

        switch (syntax.Operator)
        {
            case ArcOperator.Plus:
                result.Instructions.Add(new ArcArithmeticAddInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.Minus:
                result.Instructions.Add(new ArcArithmeticSubtractInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.Multiply:
                result.Instructions.Add(new ArcArithmeticMultiplyInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.Divide:
                result.Instructions.Add(new ArcArithmeticDivideInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.Modulus:
                result.Instructions.Add(new ArcArithmeticModuloInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.BitwiseAnd:
                result.Instructions.Add(new ArcArithmeticBitwiseAndInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.BitwiseOr:
                result.Instructions.Add(new ArcArithmeticBitwiseOrInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.BitwiseXor:
                result.Instructions.Add(new ArcArithmeticBitwiseXorInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.LogicalAnd:
                result.Instructions.Add(new ArcLogicalAndInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.LogicalOr:
                result.Instructions.Add(new ArcLogicalOrInstruction
                {
                    SrcA = lhsResult.OutImplicitRegister.Id,
                    SrcB = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id
                });
                break;
            case ArcOperator.ObjectEquals:
                result.Instructions.Add(new ArcComparisonEqualInstruction
                {
                    SrcL = lhsResult.OutImplicitRegister.Id,
                    SrcR = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                    CompareReference = false
                });
                break;
            case ArcOperator.ReferenceEquals:
                result.Instructions.Add(new ArcComparisonEqualInstruction
                {
                    SrcL = lhsResult.OutImplicitRegister.Id,
                    SrcR = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                    CompareReference = true
                });
                break;
            case ArcOperator.ObjectNotEquals:
                result.Instructions.Add(new ArcComparisonNotEqualInstruction
                {
                    SrcL = lhsResult.OutImplicitRegister.Id,
                    SrcR = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                    CompareReference = false
                });
                break;
            case ArcOperator.ReferenceNotEquals:
                result.Instructions.Add(new ArcComparisonNotEqualInstruction
                {
                    SrcL = lhsResult.OutImplicitRegister.Id,
                    SrcR = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                    CompareReference = true
                });
                break;
            case ArcOperator.LessThan:
                result.Instructions.Add(new ArcComparisonLessThanInstruction
                {
                    SrcL = lhsResult.OutImplicitRegister.Id,
                    SrcR = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                });
                break;
            case ArcOperator.LessThanOrEqual:
                result.Instructions.Add(new ArcComparisonLessThanOrEqualInstruction
                {
                    SrcL = lhsResult.OutImplicitRegister.Id,
                    SrcR = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                });
                break;
            case ArcOperator.GreaterThan:
                result.Instructions.Add(new ArcComparisonGreaterThanInstruction
                {
                    SrcL = lhsResult.OutImplicitRegister.Id,
                    SrcR = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                });
                break;
            case ArcOperator.GreaterThanOrEqual:
                result.Instructions.Add(new ArcComparisonGreaterThanOrEqualInstruction
                {
                    SrcL = lhsResult.OutImplicitRegister.Id,
                    SrcR = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                });
                break;
            case ArcOperator.ShiftLeft:
                result.Instructions.Add(new ArcArithmeticLeftShiftInstruction()
                {
                    Src = lhsResult.OutImplicitRegister.Id,
                    Amount = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                });
                break;
            case ArcOperator.ShiftRight:
                result.Instructions.Add(new ArcArithmeticRightShiftInstruction
                {
                    Src = lhsResult.OutImplicitRegister.Id,
                    Amount = rhsResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                });
                break;
            default:
                result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Critical, 0, "Unreachable compiler branch in processing binary expression", ctx.UnitName, syntax.Context));
                break;
        }
        
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult GenerateUnaryOperation(
        ref ArcFunctionGenerationContext ctx,
        ArcExpression syntax,
        ArcDataDeclarator? topLevelTarget = null)
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        
        var src = syntax.SubExpressions.ElementAt(0);
        var srcResult = Generate(ref ctx, src);
        result.Append(srcResult);
        
        var (outObj, outLogs) = topLevelTarget is null
            ? ArcImplicitRegister.CreateAnonymous(ctx, srcResult.OutImplicitRegister.Instance, ArcImplicitRegisterType.LocalData)
            : ArcImplicitRegister.Create(ctx, topLevelTarget, ArcImplicitRegisterType.LocalData);
        
        if (outObj is null)
        {
            result.Logs.AddRange(outLogs);
            return result;
        }
        
        ctx.AddImplicitRegister(outObj);
        
        switch (syntax.Operator)
        {
            case ArcOperator.BitwiseNot:
                result.Instructions.Add(new ArcArithmeticBitwiseNotInstruction
                {
                    Src = srcResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                });
                break;
            case ArcOperator.LogicalNot:
                result.Instructions.Add(new ArcLogicalNotInstruction
                {
                    Src = srcResult.OutImplicitRegister.Id,
                    Dest = outObj.Id,
                });
                break;
            default:
                result.Logs.Add(new ArcSourceLocatableLog(LogLevel.Critical, 0, "Unreachable compiler branch in processing unary expression", ctx.UnitName, syntax.Context));
                break;
        }
        
        return result;
    }
}