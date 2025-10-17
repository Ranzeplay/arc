using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcExpressionEvaluationGenerator
    {
        public static ArcPartialGenerationResult GenerateEvaluationCommand(ArcGenerationSource source, ArcExpression expr, ArcScopeTreeFunctionNodeBase baseFn)
        {
            var result = new ArcPartialGenerationResult();

            foreach (var term in expr.Terms)
            {
                if (term.IsOperator)
                {
                    result.Append(GenerateOperator(source, term.Operator!.Value, term));
                }
                else
                {
                    var logs = GenerateDataValue(ref source, ref result, term, baseFn);
                    result.Logs.AddRange(logs);
                }
            }

            return result;
        }

        private static IEnumerable<ArcCompilationLogBase> GenerateDataValue(ref ArcGenerationSource source, ref ArcPartialGenerationResult result, ArcExpressionTerm term, ArcScopeTreeFunctionNodeBase baseFn)
        {
            switch (term.DataValue?.Type)
            {
                case ArcDataValue.ValueType.InstantValue:
                {
                    var dataLocation = ArcConstantHelper.GetConstantIdOrCreateConstant(term.DataValue.InstantValue!, ref source, ref result);
                    var locator = new ArcStackDataOperationDescriptor(ArcDataSourceType.ConstantTable, dataLocation, false);

                    result.Append(new ArcLoadDataToStackInstruction(locator).Encode(source));
                    break;
                }
                case ArcDataValue.ValueType.CallChain:
                {
                    result.Append(ArcCallChainGenerator.Generate(source, term.DataValue.CallChain!, baseFn));
                    break;
                }
                case ArcDataValue.ValueType.Lambda:
                {
                    // Create a new lambda node under the current function
                    var lambda = term.DataValue.Lambda!;
                    
                    var (node, logs) = ArcFunctionGenerator.GenerateDescriptor<ArcScopeTreeLambdaNode>(source, lambda.Declarator);
                    result.Logs.AddRange(logs);

                    node.SyntaxTree = lambda;
                    var lambdaGenResult = ArcFunctionGenerator.GenerateFunction<ArcSourceCodeParser.Arc_lambda_expressionContext, ArcScopeTreeLambdaNode, ArcFunctionMinimalDeclarator>(source, node, lambda);
                    node.BlockLength = lambdaGenResult.TotalGeneratedDataSlotCount;
                    
                    baseFn.AddChild(node);
                    
                    // Push lambda symbol onto the stack
                    var locator = new ArcStackDataOperationDescriptor(ArcDataSourceType.Symbol, node.Id, false);
                    result.Append(new ArcLoadDataToStackInstruction(locator).Encode(source));
                    
                    break;
                }
                default:
                    return [new ArcSourceLocatableLog(LogLevel.Error, 0, "Data value not implemented", source.Name, term.Context)];
            }

            return [];
        }

        private static ArcPartialGenerationResult GenerateOperator(ArcGenerationSource source, ArcOperator op, ArcExpressionTerm term)
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
                _ => ArcPartialGenerationResult.WithLogs([new ArcSourceLocatableLog(LogLevel.Error, 0, "Operator not implemented", source.Name, term.Context)]),
            };
        }
    }
}
