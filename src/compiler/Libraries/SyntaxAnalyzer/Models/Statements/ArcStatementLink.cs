using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementLink(ArcSourceCodeParser.Arc_stmt_linkContext source)
    {
        public ArcScopedIdentifier Identifier { get; set; } = new ArcScopedIdentifier(source.arc_scoped_identifier());
    }
}
