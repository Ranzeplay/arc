using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal class ArcSingleIdentifier
    {
        public string Name { get; set; }

        public ArcSingleIdentifier(ArcSourceCodeParser.Arc_single_identifierContext context)
        {
            Name = context.IDENTIFIER().GetText();
        }
    }
}
