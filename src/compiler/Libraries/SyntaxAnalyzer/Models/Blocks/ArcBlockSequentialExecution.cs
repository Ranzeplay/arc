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
            }
        }
    }
}
