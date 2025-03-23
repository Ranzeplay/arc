using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain
{
    public class ArcCallChain(ArcSourceCodeParser.Arc_call_chainContext context)
    {
        public IEnumerable<ArcCallChainTerm> Terms { get; set; } = context.arc_call_chain_term().Select(term => new ArcCallChainTerm(term));

        public ArcConstructorCall? ConstructorCall { get; set; } = context.arc_constructor_call() != null ? new(context.arc_constructor_call()) : null;

        public ArcSourceCodeParser.Arc_call_chainContext Context { get; set; } = context;
    }
}
