using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal class ArcFlexibleIdentifier
    {
        public IEnumerable<string>? Namespace { get; set; }

        public string Name { get; set; }

        public ArcFlexibleIdentifier(ArcSourceCodeParser.Arc_flexible_identifierContext context)
        {
            if (context.arc_full_identifier() != null)
            {
                Namespace = context.arc_full_identifier().arc_namespace_limiter().arc_namespace_identifier().IDENTIFIER().Select(i => i.GetText());
                Name = context.arc_full_identifier().arc_single_identifier().IDENTIFIER().GetText();
            }
            else
            {
                Name = context.arc_single_identifier().IDENTIFIER().GetText();
            }
        }

        public ArcFlexibleIdentifier(ArcSourceCodeParser.Arc_single_identifierContext context)
        {
            Name = context.IDENTIFIER().GetText();
        }
    }
}
