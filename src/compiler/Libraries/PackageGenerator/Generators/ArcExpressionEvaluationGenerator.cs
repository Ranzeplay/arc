using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcExpressionEvaluationGenerator
    {
        public static ArcPartialGenerationResult GenerateEvaluationCommand(ArcGenerationSource source, ArcExpression expr)
        {
            var result = new ArcPartialGenerationResult();

            foreach (var term in expr.Terms)
            {
                if (term.IsOperator)
                {
                    switch (term.Operator)
                    {
                        case ArcOperator.Plus:
                            result.Append(new ArcAddInstruction().Encode(source));
                            break;
                        case ArcOperator.Minus:
                            result.Append(new ArcSubtractInstruction().Encode(source));
                            break;
                        case ArcOperator.Multiply:
                            result.Append(new ArcMultiplyInstruction().Encode(source));
                            break;
                        case ArcOperator.Divide:
                            result.Append(new ArcDivideInstruction().Encode(source));
                            break;
                        case ArcOperator.Modulus:
                            result.Append(new ArcModulusInstruction().Encode(source));
                            break;
                        case ArcOperator.BitwiseAnd:
                            result.Append(new ArcBitwiseAndInstruction().Encode(source));
                            break;
                        case ArcOperator.BitwiseOr:
                            result.Append(new ArcBitwiseOrInstruction().Encode(source));
                            break;
                        case ArcOperator.BitwiseXor:
                            result.Append(new ArcBitwiseXorInstruction().Encode(source));
                            break;
                        case ArcOperator.BitwiseNot:
                            result.Append(new ArcBitwiseNotInstruction().Encode(source));
                            break;
                        case ArcOperator.ShiftLeft:
                            result.Append(new ArcShiftLeftInstruction().Encode(source));
                            break;
                        case ArcOperator.ShiftRight:
                            result.Append(new ArcShiftRightInstruction().Encode(source));
                            break;
                        case ArcOperator.ObjectEquals:
                            result.Append(new ArcComparisonValueEqualInstruction().Encode(source));
                            break;
                        case ArcOperator.ObjectNotEquals:
                            result.Append(new ArcComparisonValueNotEqualInstruction().Encode(source));
                            break;
                        case ArcOperator.ReferenceEquals:
                            result.Append(new ArcComparisonReferenceEqualInstruction().Encode(source));
                            break;
                        case ArcOperator.ReferenceNotEquals:
                            result.Append(new ArcComparisonReferenceNotEqualInstruction().Encode(source));
                            break;
                        case ArcOperator.LessThan:
                            result.Append(new ArcComparisonLessThanInstruction().Encode(source));
                            break;
                        case ArcOperator.LessThanOrEqual:
                            result.Append(new ArcComparisonLessThanOrEqualToInstruction().Encode(source));
                            break;
                        case ArcOperator.GreaterThan:
                            result.Append(new ArcComparisonMoreThanInstruction().Encode(source));
                            break;
                        case ArcOperator.GreaterThanOrEqual:
                            result.Append(new ArcComparisonMoreThanOrEqualToInstruction().Encode(source));
                            break;
                        case ArcOperator.LogicalAnd:
                            result.Append(new ArcLogicalAndInstruction().Encode(source));
                            break;
                        case ArcOperator.LogicalOr:
                            result.Append(new ArcLogicalOrInstruction().Encode(source));
                            break;
                        case ArcOperator.LogicalNot:
                            result.Append(new ArcLogicalNotInstruction().Encode(source));
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    switch (term.DataValue?.Type)
                    {
                        case ArcDataValue.ValueType.InstantValue:
                            {
                                var dataLocation = Utils.GetConstantIdOrCreateConstant(term.DataValue.InstantValue!, ref source, ref result);
                                result.Append(new ArcLoadDataToStackInstruction(ArcDataSourceType.ConstantTable, dataLocation, 0x00).Encode(source));
                                break;
                            }
                        case ArcDataValue.ValueType.FunctionCall:
                            {
                                result.Append(ArcFunctionCallGenerator.Generate(source, term.DataValue.FunctionCall!));
                                break;
                            }
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            return result;
        }
    }
}
