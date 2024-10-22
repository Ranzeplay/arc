using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    public class ArcFullIdentifier(ArcSourceCodeParser.Arc_full_identifierContext context)
        : IArcTraceable<ArcSourceCodeParser.Arc_full_identifierContext>
    {
        public ArcNamespaceIdentifier? Namespace { get; set; } = new(context.arc_namespace_limiter().arc_namespace_identifier());

        public string Name { get; set; } = context.arc_single_identifier().IDENTIFIER().GetText();

        public ArcSourceCodeParser.Arc_full_identifierContext Context { get; } = context;
    }
}
