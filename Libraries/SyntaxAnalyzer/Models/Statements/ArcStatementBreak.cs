using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementBreak(ArcSourceCodeParser.Arc_break_stmtContext context) : ArcExecutionStepBase
    {
        public ArcSourceCodeParser.Arc_break_stmtContext Context { get; } = context;
    }
}
