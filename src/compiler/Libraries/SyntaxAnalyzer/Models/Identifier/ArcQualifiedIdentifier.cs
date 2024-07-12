using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal class ArcQualifiedIdentifier : ArcIdentifierBase
    {
        public ArcQualifiedIdentifier(ArcSourceCodeParser.Arc_qualified_identifierContext source)
        {
            Names = source.IDENTIFIER().Select(ident => ident.GetText()).ToList();
        }
    }
}
