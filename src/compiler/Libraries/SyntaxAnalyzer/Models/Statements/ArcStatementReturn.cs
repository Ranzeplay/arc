using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    public class ArcStatementReturn : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_returnContext>
    {
        public ArcStatementReturn(ArcSourceCodeParser.Arc_stmt_returnContext context)
        {
            if (context.arc_expression() != null)
            {
                Expression = new ArcExpression(context.arc_expression());
            }

            Context = context;
        }

        public ArcExpression? Expression { get; set; }
        public ArcSourceCodeParser.Arc_stmt_returnContext Context { get; }
    }
}
