using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal class ArcFullIdentifier
    {
        public ArcNamespaceIdentifier? Namespace { get; set; }

        public string Name { get; set; }

        public ArcFullIdentifier(ArcSourceCodeParser.Arc_full_identifierContext context)
        {
            Namespace = new(context.arc_namespace_limiter().arc_namespace_identifier());
            Name = context.arc_single_identifier().IDENTIFIER().GetText();
        }
    }
}
