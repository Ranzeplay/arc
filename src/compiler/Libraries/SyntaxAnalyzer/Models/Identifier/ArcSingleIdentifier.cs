using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    public class ArcSingleIdentifier : IArcTraceable<ParserRuleContext>
    {
        public string Name { get; set; }

        public ParserRuleContext Context { get; }

        public ArcSingleIdentifier(ArcSourceCodeParser.Arc_single_identifierContext context)
        {
            Name = context.IDENTIFIER().GetText();
            Context = context;
        }

        public ArcSingleIdentifier(ArcSourceCodeParser.Arc_self_wrapperContext context)
        {
            Name = context.KW_SELF().GetText();
            Context = context;
        }

        public override string ToString() => Name;
    }
}
