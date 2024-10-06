using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Primitives;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ExpressionEvaluator
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
                            result.Append(new AddInstruction().Encode(source));
                            break;
                        case ArcOperator.Minus:
                            result.Append(new SubtractInstruction().Encode(source));
                            break;
                        case ArcOperator.Multiply:
                            result.Append(new MultiplyInstruction().Encode(source));
                            break;
                        case ArcOperator.Divide:
                            result.Append(new DivideInstruction().Encode(source));
                            break;
                        case ArcOperator.Modulus:
                            result.Append(new ModulusInstruction().Encode(source));
                            break;
                        case ArcOperator.BitwiseAnd:
                            result.Append(new BitwiseAndInstruction().Encode(source));
                            break;
                        case ArcOperator.BitwiseOr:
                            result.Append(new BitwiseOrInstruction().Encode(source));
                            break;
                        case ArcOperator.BitwiseXor:
                            result.Append(new BitwiseXorInstruction().Encode(source));
                            break;
                        case ArcOperator.BitwiseNot:
                            result.Append(new BitwiseNotInstruction().Encode(source));
                            break;
                        case ArcOperator.ShiftLeft:
                            result.Append(new ShiftLeftInstruction().Encode(source));
                            break;
                        case ArcOperator.ShiftRight:
                            result.Append(new ShiftRightInstruction().Encode(source));
                            break;
                        case ArcOperator.ObjectEquals:
                            result.Append(new ComparisonValueEqualInstruction().Encode(source));
                            break;
                        case ArcOperator.ObjectNotEquals:
                            result.Append(new ComparisonValueNotEqualInstruction().Encode(source));
                            break;
                        case ArcOperator.ReferenceEquals:
                            result.Append(new ComparisonReferenceEqualInstruction().Encode(source));
                            break;
                        case ArcOperator.ReferenceNotEquals:
                            result.Append(new ComparisonReferenceNotEqualInstruction().Encode(source));
                            break;
                        case ArcOperator.LessThan:
                            result.Append(new ComparisonLessThanInstruction().Encode(source));
                            break;
                        case ArcOperator.LessThanOrEqual:
                            result.Append(new ComparisonLessThanOrEqualToInstruction().Encode(source));
                            break;
                        case ArcOperator.GreaterThan:
                            result.Append(new ComparisonMoreThanInstruction().Encode(source));
                            break;
                        case ArcOperator.GreaterThanOrEqual:
                            result.Append(new ComparisonMoreThanOrEqualToInstruction().Encode(source));
                            break;
                        case ArcOperator.LogicalAnd:
                            result.Append(new LogicalAndInstruction().Encode(source));
                            break;
                        case ArcOperator.LogicalOr:
                            result.Append(new LogicalOrInstruction().Encode(source));
                            break;
                        case ArcOperator.LogicalNot:
                            result.Append(new LogicalNotInstruction().Encode(source));
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
                            result.Append(new PushInstantValueInstruction(term.DataValue.InstantValue!).Encode(source));
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            return result;
        }
    }
}
