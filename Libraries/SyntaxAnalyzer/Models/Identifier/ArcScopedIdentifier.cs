using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal class ArcScopedIdentifier : IdentifierBase
    {
        public ArcScopedIdentifier(ArcSourceCodeParser.Arc_scoped_identifierContext source)
        {
            Names = source.IDENTIFIER().Select(ident => ident.GetText()).ToList();
        }

        public string Domain { get => Names[0]; set => Names[0] = value; }

        public IEnumerable<string> Scopes
        {
            get => Names.Skip(1).SkipLast(1);
            set
            {
                var result = new List<string>
                {
                    Domain, Name
                };
                result.InsertRange(1, value);
                Names = result;
            }
        }

        public string Name { get => Names[^1]; set => Names[Names.Count - 1] = value; }
    }
}
