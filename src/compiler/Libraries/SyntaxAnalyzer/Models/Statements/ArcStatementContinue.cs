using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementContinue(ArcSourceCodeParser.Arc_continue_stmtContext context)
    {
        public ArcSourceCodeParser.Arc_continue_stmtContext Context { get; } = context;
    }
}
