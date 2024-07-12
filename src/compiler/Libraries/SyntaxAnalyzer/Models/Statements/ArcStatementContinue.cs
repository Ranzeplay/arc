using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementContinue(ArcSourceCodeParser.Arc_stmt_continueContext context)
    {
        public ArcSourceCodeParser.Arc_stmt_continueContext Context { get; } = context;
    }
}
