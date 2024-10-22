using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal static class ArcSequentialExecutionGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcBlockSequentialExecution seqExec)
        {
            var result = new ArcPartialGenerationResult();

            foreach (var step in seqExec.ExecutionSteps)
            {
                var stepResult = new ArcPartialGenerationResult();
                switch (step)
                {
                    case ArcStatementDeclaration decl:
                        {
                            stepResult = new ArcDeclarationInstruction(decl.DataDeclarator).Encode(source);
                            break;
                        }
                    case ArcStatementAssign assign:
                        {
                            stepResult = ArcAssignmentStatementGenerator.Generate(assign, source);
                            break;
                        }
                    case ArcBlockIf ifBlock:
                        {
                            stepResult = ArcConditionBlockGenerator.Encode(source, ifBlock);
                            break;
                        }
                    case ArcBlockConditionalLoop conditionalLoop:
                        {
                            stepResult = ArcConditionLoopBlockGenerator.Encode(source, conditionalLoop);
                            break;
                        }
                    case ArcStatementReturn @return:
                        {
                            stepResult = ArcSequenceReturnGenerator.Generate(source, @return);
                            break;
                        }
                    case ArcStatementBreak @break:
                        {
                            stepResult = ArcLoopControlGenerator.GenerateBreak(source);
                            break;
                        }
                    case ArcStatementContinue @continue:
                        {
                            stepResult = ArcLoopControlGenerator.GenerateContinue(source);
                            break;
                        }
                    case ArcStatementCall call:
                        {
                            stepResult = ArcFunctionCallGenerator.Generate(source, call.FunctionCall);
                            // Discard the result of the function call
                            stepResult.Append(new ArcDiscardStackTopInstruction().Encode(source));
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
