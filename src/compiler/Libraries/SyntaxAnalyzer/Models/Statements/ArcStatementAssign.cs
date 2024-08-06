using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementAssign(ArcSourceCodeParser.Arc_stmt_assignContext context) : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_assignContext>
    {
        public ArcFlexibleIdentifier Identifier { get; set; } = new(context.arc_flexible_identifier());

        public ArcExpression Expression { get; set; } = new(context.arc_expression());

        public bool AssignIfNull { get; set; } = context.ASSIGN_IF_NULL() != null;
        
        public ArcSourceCodeParser.Arc_stmt_assignContext Context { get; } = context;
    }
}
