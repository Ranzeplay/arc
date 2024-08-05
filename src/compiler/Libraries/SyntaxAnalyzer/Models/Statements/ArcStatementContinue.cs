using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementContinue(ArcSourceCodeParser.Arc_stmt_continueContext context) : ArcExecutionStepBase
    {
        public ArcSourceCodeParser.Arc_stmt_continueContext Context { get; } = context;
    }
}
