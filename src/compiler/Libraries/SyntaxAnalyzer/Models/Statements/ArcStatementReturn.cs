using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementReturn
    {
        public ArcStatementReturn(ArcSourceCodeParser.Arc_stmt_returnContext context)
        {
            if (context.arc_expression() != null)
            {
                Expression = new ArcExpression(context.arc_expression());
            }
        }

        public ArcExpression? Expression { get; set; }
    }
}
