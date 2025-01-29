using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
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
                    result.Append(GenerateOperator(source, term.Operator!.Value));
                }
                else
                {
                    GenerateDataValue(ref source, ref result, term);
                }
            }

            return result;
        }

        private static void GenerateDataValue(ref ArcGenerationSource source, ref ArcPartialGenerationResult result, ArcExpressionTerm term)
        {
            switch (term.DataValue?.Type)
            {
                case ArcDataValue.ValueType.InstantValue:
                    {
                        var dataLocation = Utils.GetConstantIdOrCreateConstant(term.DataValue.InstantValue!, ref source, ref result);
                        var locator = new ArcDataLocator(ArcDataSourceType.ConstantTable, dataLocation, [], []);

                        result.Append(new ArcLoadDataToStackInstruction(locator).Encode(source));
                        break;
                    }
                case ArcDataValue.ValueType.CallChain:
                    {
                        result.Append(ArcCallChainGenerator.Generate(source, term.DataValue.CallChain!));
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private static ArcPartialGenerationResult GenerateOperator(ArcGenerationSource source, ArcOperator op)
        {
            return op switch
            {
                ArcOperator.Plus => new ArcAddInstruction().Encode(source),
                ArcOperator.Minus => new ArcSubtractInstruction().Encode(source),
                ArcOperator.Multiply => new ArcMultiplyInstruction().Encode(source),
                ArcOperator.Divide => new ArcDivideInstruction().Encode(source),
                ArcOperator.Modulus => new ArcModulusInstruction().Encode(source),
                ArcOperator.BitwiseAnd => new ArcBitwiseAndInstruction().Encode(source),
                ArcOperator.BitwiseOr => new ArcBitwiseOrInstruction().Encode(source),
                ArcOperator.BitwiseXor => new ArcBitwiseXorInstruction().Encode(source),
                ArcOperator.BitwiseNot => new ArcBitwiseNotInstruction().Encode(source),
                ArcOperator.ShiftLeft => new ArcShiftLeftInstruction().Encode(source),
                ArcOperator.ShiftRight => new ArcShiftRightInstruction().Encode(source),
                ArcOperator.ObjectEquals => new ArcComparisonValueEqualInstruction().Encode(source),
                ArcOperator.ObjectNotEquals => new ArcComparisonValueNotEqualInstruction().Encode(source),
                ArcOperator.ReferenceEquals => new ArcComparisonReferenceEqualInstruction().Encode(source),
                ArcOperator.ReferenceNotEquals => new ArcComparisonReferenceNotEqualInstruction().Encode(source),
                ArcOperator.LessThan => new ArcComparisonLessThanInstruction().Encode(source),
                ArcOperator.LessThanOrEqual => new ArcComparisonLessThanOrEqualToInstruction().Encode(source),
                ArcOperator.GreaterThan => new ArcComparisonMoreThanInstruction().Encode(source),
                ArcOperator.GreaterThanOrEqual => new ArcComparisonMoreThanOrEqualToInstruction().Encode(source),
                ArcOperator.LogicalAnd => new ArcLogicalAndInstruction().Encode(source),
                ArcOperator.LogicalOr => new ArcLogicalOrInstruction().Encode(source),
                ArcOperator.LogicalNot => new ArcLogicalNotInstruction().Encode(source),
                _ => throw new NotImplementedException("Other operators are not available for now"),
            };
        }
    }
}
