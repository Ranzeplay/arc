using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementAssign
    {
        public ArcFlexibleIdentifier Identifier { get; set; }

        public ArcExpression Expression { get; set; }

        // TODO
        public bool AssignIfNull { get; set; }

        public ArcStatementAssign(ArcSourceCodeParser.Arc_stmt_assignContext context)
        {
            Identifier = new(context.arc_flexible_identifier());
            Expression = new(context.arc_expression());
        }
    }
}
