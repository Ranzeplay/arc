using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal static class ArcSequentialExecutionGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcBlockSequentialExecution seqExec, ArcScopeTreeFunctionNodeBase fnNode)
        {
            var result = new ArcPartialGenerationResult();

            foreach (var step in seqExec.ExecutionSteps)
            {
                var stepResult = new ArcPartialGenerationResult();
                switch (step)
                {
                    case ArcStatementDeclaration decl:
                        {
                            stepResult = ArcDeclarationStatementGenerator.Generate(decl, source, fnNode);
                            break;
                        }
                    case ArcStatementAssign assign:
                        {
                            stepResult = assign.Generate(source, fnNode);
                            break;
                        }
                    case ArcBlockIf ifBlock:
                        {
                            stepResult = ArcConditionBlockGenerator.Encode(source, ifBlock, fnNode);
                            break;
                        }
                    case ArcBlockConditionalLoop conditionalLoop:
                        {
                            stepResult = ArcConditionLoopBlockGenerator.EncodeWhileLoop(source, conditionalLoop, fnNode);
                            break;
                        }
                    case ArcStatementReturn ret:
                        {
                            stepResult = ArcSequenceReturnGenerator.Generate(source, ret, fnNode);
                            break;
                        }
                    case ArcStatementBreak:
                        {
                            stepResult = ArcLoopControlGenerator.GenerateBreak(source);
                            break;
                        }
                    case ArcStatementContinue:
                        {
                            stepResult = ArcLoopControlGenerator.GenerateContinue(source);
                            break;
                        }
                    case ArcStatementCall call:
                        {
                            if (call.FunctionCall == null && call.CallChain?.Terms.LastOrDefault()?.Type != ArcCallChainTermType.FunctionCall)
                            {
                                stepResult.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Expected a function call clause", source.Name, call.Context));
                                break;
                            }

                            ulong? funcId;
                            IEnumerable<ArcCompilationLogBase> logs;
                            if (call.FunctionCall != null)
                            {
                                stepResult = ArcFunctionCallGenerator.Generate(source, call.FunctionCall, false, true, fnNode);
                                (funcId, logs) = ArcFunctionHelper.GetFunctionId(source, call.FunctionCall);
                            }
                            else
                            {
                                (stepResult, funcId) = ArcCallChainGenerator.GenerateWithFinalCalledFunctionId(source, call.CallChain, fnNode);
                                logs = [];
                            }
                            
                            // Discard the result of the function call if any
                            stepResult.Logs.AddRange(logs);
                            var function = source.GlobalScopeTree
                                .FlattenedNodes
                                .OfType<ArcScopeTreeFunctionNodeBase>()
                                .FirstOrDefault(f => f.Id == funcId);

                            if (function == null)
                            {
                                stepResult.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Function not found", source.Name, call.FunctionCall.Context));
                            }

                            if (function?.ReturnValueType.Type.TypeId != 0)
                            {
                                stepResult.Append(new ArcDiscardStackTopInstruction().Encode(source));
                            }
                            break;
                        }
                    case ArcStatementThrow stmtThrow:
                        {
                            stepResult.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, stmtThrow.Expression, fnNode));
                            stepResult.Append(new ArcThrowInstruction().Encode(source));
                            break;
                        }
                    case ArcBlockExtendedConditionalLoop forLoop:
                        {
                            stepResult = ArcConditionLoopBlockGenerator.EncodeForLoop(source, forLoop, fnNode);
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }

                source.Merge(stepResult);
                result.Append(stepResult);
            }

            result.DataSlots = [];
            return result;
        }
    }
}
