using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
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
                if (term.Value is ArcOperator op)
                {
                    result.Append(GenerateOperator(source, op, term));
                }
                else
                {
                    var logs = term.Value is ArcDataValue value
                        ? GenerateDataValue(ref source, ref result, value, term, baseFn)
                        : throw new InvalidOperationException("Uninitialized expression term");
                    result.Logs.AddRange(logs);
                }
            }

            return result;
        }

        private static IEnumerable<ArcCompilationLogBase> GenerateDataValue(ref ArcGenerationSource source, ref ArcPartialGenerationResult result, ArcDataValue value, ArcExpressionTerm term, ArcScopeTreeFunctionNodeBase baseFn)
        {
            switch (value)
            {
                case ArcInstantValue instant:
                {
                    var dataLocation = ArcConstantHelper.GetConstantIdOrCreateConstant(instant, ref source, ref result);
                    var locator = new ArcStackDataOperationDescriptor(ArcDataSourceType.ConstantTable, dataLocation, false);

                    result.Append(new ArcLoadDataToStackInstruction(locator).Encode(source));
                    break;
                }
                case ArcCallChain chain:
                {
                    result.Append(ArcCallChainGenerator.Generate(source, chain, baseFn));
                    break;
                }
                case ArcLambdaExpression lambda:
                {
                    // Create a new lambda node under the current function
                    
                    var (node, logs) = ArcFunctionGenerator.GenerateDescriptor<ArcScopeTreeLambdaNode>(source, lambda.Declarator);
                    result.Logs.AddRange(logs);
                    
                    baseFn.AddChild(node);
                    source.GlobalScopeTree.FlattenedNodes.First(n => n.Id == baseFn.Id).AddChild(node);

                    node.SyntaxTree = lambda;
                    var lambdaSource = source.Clone();
                    lambdaSource.LocalDataSlots = [];
                    var lambdaGenResult = ArcFunctionGenerator.GenerateFunction<ArcSourceCodeParser.Arc_lambda_expressionContext, ArcScopeTreeLambdaNode, ArcFunctionMinimalDeclarator>(lambdaSource, node, lambda, true);
                    node.BlockLength = lambdaGenResult.TotalGeneratedDataSlotCount;
                    node.GenerationResult = lambdaGenResult;
                    node.Parent = baseFn;
                    
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
