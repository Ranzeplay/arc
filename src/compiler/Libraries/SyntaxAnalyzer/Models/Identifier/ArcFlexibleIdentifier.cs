using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    public class ArcFlexibleIdentifier : IArcTraceable<ParserRuleContext>
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

            Context = context;
        }

        public ArcFlexibleIdentifier(ArcSourceCodeParser.Arc_single_identifierContext context)
        {
            Name = context.IDENTIFIER().GetText();
            Context = context;
        }

        public ParserRuleContext Context { get; }

        public override string? ToString()
        {
            return Namespace != null ? $"{string.Join(":", Namespace)}+{Name}" : Name;
        }

        public string AsFunctionIdentifier()
        {
            return Namespace != null ? $"{string.Join(":", Namespace)}+F{Name}" : Name;
        }

        public IEnumerable<string> NameArray => Namespace != null ? Namespace.Append(Name) : [Name];
    }
}
