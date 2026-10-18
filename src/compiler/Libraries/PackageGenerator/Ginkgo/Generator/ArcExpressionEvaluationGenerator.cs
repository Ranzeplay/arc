using System.Diagnostics;
using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
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
        var result = GenerateCore(ref ctx, syntax);
        if (result.DiscardResult) return result;
        if (result.OutImplicitRegister is null)
        {
            result.Append(ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Expression does not produce a Ginkgo value", ctx.UnitName, syntax));
            return result;
        }
        
        if (topLevelTarget is null)
        {
            return result;
        }
        
        var source = result.OutImplicitRegister;
        var (target, logs) = topLevelTarget.DataType.PrimitiveType == ArcPrimitiveDataType.Infer
            ? (new ArcImplicitRegister(ctx.NextImplicitRegisterId, topLevelTarget.Identifier.Name,
                new ArcDataTypeInstance { Type = source.Instance.Type, Dimension = source.Instance.Dimension,
                    AllowNone = source.Instance.AllowNone, Mutability = topLevelTarget.Mutability },
                ArcImplicitRegisterType.LocalData), Enumerable.Empty<ArcCompilationLogBase>())
            : ArcImplicitRegister.Create(ctx, topLevelTarget, ArcImplicitRegisterType.LocalData);
        result.Logs.AddRange(logs);
        
        if (target is null)
        {
            return result;
        }
        
        if (target.Instance.Type.ActualTypeId != source.Instance.Type.ActualTypeId ||
            target.Instance.Dimension != source.Instance.Dimension)
        {
            result.Append(ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Initializer type does not match declaration", ctx.UnitName, syntax));
            return result;
        }
        
        ctx.AddImplicitRegister(target);
        result.StoreOutput(target);
        return result;
    }

    private static ArcGinkgoInstructionGenerationResult GenerateCore(
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
        if (syntax.CallChain is not null)
        {
            return ArcCallChainGenerator.Generate(ref ctx, syntax.CallChain);
        }
        
        var value = syntax.InstantValue;
        var typeName = value?.TypeName ?? (syntax.Type == ArcDataValue.ValueType.None ? "none" : null);
        if (typeName is null)
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                $"Ginkgo cannot generate data value {syntax.Type} yet", ctx.UnitName);
        }
        
        var type = ctx.GlobalContext.ScopeTree.GetNodes<ArcScopeTreeDataTypeNode>()
            .FirstOrDefault(t => t.ShortName == typeName);
        if (type is null)
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                $"Builtin type {typeName} is missing from the Ginkgo scope", ctx.UnitName);
        }
        
        var (output, logs) = ArcImplicitRegister.CreateAnonymous(ctx,
            new ArcDataTypeInstance { Type = type, Mutability = ArcMutability.Constant,
                AllowNone = typeName == "none" }, ArcImplicitRegisterType.LocalData);
        if (output is null) return ArcGinkgoInstructionGenerationResult.WithLogs(logs);
        ctx.AddImplicitRegister(output);
        ArcGinkgoInstructionBase instruction;
        if (value?.Type == ArcInstantValue.ValueType.Integer)
            instruction = new ArcDataLoadImmediateIntegerInstruction
                { Value = value.IntegerValue!.Value, Dest = output.Id };
        else
        {
            // Preserve decimal precision and the bool/none types in the constant pool.
            var index = ctx.GlobalContext.InternConstant(type.ActualTypeId, value?.GetRawValue() ?? Array.Empty<byte>());
            instruction = new ArcDataLoadConstantInstruction { TableIndex = index, Dest = output.Id };
        }
        
        return new ArcGinkgoInstructionGenerationResult
            { Instructions = [instruction], OutImplicitRegister = output };
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
        if (result.DiscardResult || lhsResult.OutImplicitRegister is null)
        {
            return result;
        }
        
        
        var rhs = syntax.SubExpressions.ElementAt(1);
        var rhsResult = Generate(ref ctx, rhs);
        result.Append(rhsResult);
        if (result.DiscardResult || rhsResult.OutImplicitRegister is null)
        {
            return result;
        }

        var leftType = lhsResult.OutImplicitRegister.Instance;
        var rightType = rhsResult.OutImplicitRegister.Instance;
        if (leftType.Type.ActualTypeId != rightType.Type.ActualTypeId || leftType.Dimension != rightType.Dimension)
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Binary operands must have matching types", ctx.UnitName, syntax);
        }
        if ((syntax.Operator is ArcOperator.LogicalAnd or ArcOperator.LogicalOr) &&
            (leftType.Type.ShortName != "bool" || leftType.Dimension != 0))
        {
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Logical operands must be scalar bool values", ctx.UnitName, syntax);
        }
        
        var (outObj, outLogs) = topLevelTarget is null
            ? ArcImplicitRegister.CreateAnonymous(ctx, lhsResult.OutImplicitRegister.Instance, ArcImplicitRegisterType.LocalData)
            : ArcImplicitRegister.Create(ctx, topLevelTarget, ArcImplicitRegisterType.LocalData);

        if (outObj is null)
        {
            result.Logs.AddRange(outLogs);
            return result;
        }

        if (syntax.Operator?.IsComparison() ?? syntax.Operator is
                ArcOperator.LogicalAnd or ArcOperator.LogicalOr)
        {
            var boolean = ctx.GlobalContext.ScopeTree.GetNodes<ArcScopeTreeDataTypeNode>()
                .FirstOrDefault(t => t.ShortName == "bool");
            if (boolean is null)
                return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0, "Missing bool type", ctx.UnitName);
            outObj.Instance = new ArcDataTypeInstance { Type = boolean, Mutability = ArcMutability.Constant };
        }
        ctx.AddImplicitRegister(outObj);
        result.OutImplicitRegister = outObj;

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
        if (result.DiscardResult || srcResult.OutImplicitRegister is null) return result;
        if (syntax.Operator == ArcOperator.LogicalNot &&
            (srcResult.OutImplicitRegister.Instance.Type.ShortName != "bool" ||
             srcResult.OutImplicitRegister.Instance.Dimension != 0))
            return ArcGinkgoInstructionGenerationResult.WithLog(LogLevel.Error, 0,
                "Logical operand must be a scalar bool", ctx.UnitName, syntax);
        
        var (outObj, outLogs) = topLevelTarget is null
            ? ArcImplicitRegister.CreateAnonymous(ctx, srcResult.OutImplicitRegister.Instance, ArcImplicitRegisterType.LocalData)
            : ArcImplicitRegister.Create(ctx, topLevelTarget, ArcImplicitRegisterType.LocalData);
        
        if (outObj is null)
        {
            result.Logs.AddRange(outLogs);
            return result;
        }
        
        ctx.AddImplicitRegister(outObj);
        result.OutImplicitRegister = outObj;
        
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
