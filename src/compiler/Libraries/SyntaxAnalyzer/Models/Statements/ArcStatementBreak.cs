using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    public class ArcStatementBreak(ArcSourceCodeParser.Arc_stmt_breakContext context) : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_breakContext>
    {
        public ArcSourceCodeParser.Arc_stmt_breakContext Context { get; } = context;
    }
}
