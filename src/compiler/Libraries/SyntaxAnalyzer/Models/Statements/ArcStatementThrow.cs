using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    public class ArcStatementThrow : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_throwContext>
    {
        public ArcExpression Expression { get; set; }

        public ArcSourceCodeParser.Arc_stmt_throwContext Context { get; }

        public ArcStatementThrow(ArcSourceCodeParser.Arc_stmt_throwContext context)
        {
            Context = context;
            Expression = new ArcExpression(context.arc_expression());
        }
    }
}
