using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockSequentialExecution
    {
        public List<ArcExecutionStepBase> ExecutionSteps { get; set; } = [];

        public ArcBlockSequentialExecution(ArcSourceCodeParser.Arc_wrapped_function_bodyContext context)
        {
            foreach (var entry in context.arc_statement())
            {
                if (entry.arc_stmt_break() != null)
                {
                    ExecutionSteps.Add(new ArcStatementBreak(entry.arc_stmt_break()));
                }
                else if (entry.arc_stmt_continue() != null)
                {
                    ExecutionSteps.Add(new ArcStatementContinue(entry.arc_stmt_continue()));
                }
                else if (entry.arc_stmt_assign() != null)
                {
                    ExecutionSteps.Add(new ArcStatementAssign(entry.arc_stmt_assign()));
                }
                else if (entry.arc_stmt_decl() != null)
                {
                    ExecutionSteps.Add(new ArcStatementDeclaration(entry.arc_stmt_decl()));
                }
                else if (entry.arc_stmt_while() != null)
                {
                    ExecutionSteps.Add(new ArcBlockConditionalLoop(entry.arc_stmt_while()));
                }
                else if (entry.arc_stmt_loop() != null)
                {
                    ExecutionSteps.Add(new ArcBlockLoop(entry.arc_stmt_loop()));
                }
                else if (entry.arc_stmt_if() != null)
                {
                    ExecutionSteps.Add(new ArcBlockIf(entry.arc_stmt_if()));
                }
                else if (entry.arc_stmt_call() != null)
                {
                    ExecutionSteps.Add(new ArcStatementCall(entry.arc_stmt_call()));
                }
                else if (entry.arc_stmt_return() != null)
                {
                    ExecutionSteps.Add(new ArcStatementReturn(entry.arc_stmt_return()));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
