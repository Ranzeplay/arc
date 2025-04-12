using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    public class ArcStatementDeclaration(ArcSourceCodeParser.Arc_stmt_declContext context) : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_declContext>
    {
        public ArcDataDeclarator DataDeclarator { get; set; } = new(context.arc_data_declarator());

        public ArcExpression? InitialValueExpression { get; set; } = context.arc_expression() != null ? new ArcExpression(context.arc_expression()) : null;

        public ArcSourceCodeParser.Arc_stmt_declContext Context { get; } = context;
    }
}
