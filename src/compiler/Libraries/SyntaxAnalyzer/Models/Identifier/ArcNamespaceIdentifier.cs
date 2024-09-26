using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    public class ArcNamespaceIdentifier : IArcTraceable<ArcSourceCodeParser.Arc_namespace_identifierContext>
    {
        public IEnumerable<string>? Namespace { get; set; }

        public ArcSourceCodeParser.Arc_namespace_identifierContext Context { get; }
        
        public ArcNamespaceIdentifier(ArcSourceCodeParser.Arc_namespace_identifierContext context)
        {
            Namespace = context.IDENTIFIER().Select(i => i.GetText());
            Context = context;
        }

        public override string? ToString()
        {
            return string.Join("::", Namespace);
        }
    }
}
