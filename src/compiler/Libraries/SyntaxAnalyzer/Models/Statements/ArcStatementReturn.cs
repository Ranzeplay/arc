using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementReturn
    {
        public ArcStatementReturn(ArcSourceCodeParser.Arc_stmt_returnContext source)
        {
            if (source.arc_expression() != null)
            {
                Expression = new ArcExpression(source.arc_expression());
            }
        }   

        public ArcExpression? Expression { get; set; }
    }
}
