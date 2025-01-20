using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    public class ArcStatementAssign(ArcSourceCodeParser.Arc_stmt_assignContext context) : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_assignContext>
    {
        public ArcCallChain CallChain { get; set; } = new(context.arc_call_chain());

        public ArcExpression Expression { get; set; } = new(context.arc_expression());

        public bool AssignIfNull { get; set; } = context.ASSIGN_IF_NULL() != null;

        public ArcSourceCodeParser.Arc_stmt_assignContext Context { get; } = context;
    }
}
