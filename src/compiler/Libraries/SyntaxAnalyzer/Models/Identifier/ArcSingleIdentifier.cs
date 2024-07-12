using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal class ArcSingleIdentifier : ArcIdentifierBase
    {
        public ArcSingleIdentifier(ArcSourceCodeParser.Arc_single_identifierContext source)
        {
            Name = source.GetText();
        }

        public string Name { get => Names.ElementAt(0); set => Names = [value]; }
    }
}
