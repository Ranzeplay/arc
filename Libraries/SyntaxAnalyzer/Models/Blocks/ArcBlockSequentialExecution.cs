using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockSequentialExecution
    {
        public List<ArcExecutionStepBase> ExecutionSteps { get; set; } = [];

        public ArcBlockSequentialExecution(ArcSourceCodeParser.Arc_wrapped_bodyContext context)
        {
            foreach(var entry in context.arc_exec_step())
            {
                if(entry.arc_statement() != null)
                {
                    var statement = entry.arc_statement();
                    if(statement.arc_break_stmt() != null)
                    {
                        ExecutionSteps.Add(new ArcStatementBreak(statement.arc_break_stmt()));
                    }
                }
            }
        }
    }
}
