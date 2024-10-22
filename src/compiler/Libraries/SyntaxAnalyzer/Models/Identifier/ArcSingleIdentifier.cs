using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    public class ArcSingleIdentifier(ArcSourceCodeParser.Arc_single_identifierContext context) : IArcTraceable<ArcSourceCodeParser.Arc_single_identifierContext>
    {
        public string Name { get; set; } = context.IDENTIFIER().GetText();

        public ArcSourceCodeParser.Arc_single_identifierContext Context { get; } = context;

        public override string ToString() => Name;
    }
}
