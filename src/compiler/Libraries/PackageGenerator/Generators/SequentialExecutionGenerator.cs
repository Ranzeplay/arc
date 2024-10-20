using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal static class SequentialExecutionGenerator
    {
        public static ArcPartialGenerationResult GenerateSequentialExecutionFlow(ArcGenerationSource source, ArcBlockSequentialExecution seqExec)
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
                            stepResult = AssignmentStatement.Generate(assign, source);
                            break;
                        }
                    case ArcBlockIf ifBlock:
                        {
                            stepResult = ConditionBlockGenerator.Encode(source, ifBlock);
                            break;
                        }
                    case ArcBlockConditionalLoop conditionalLoop:
                        {
                            stepResult = ConditionLoopBlockGenerator.Encode(source, conditionalLoop);
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
