using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;
using System.Diagnostics;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain
{
    public sealed record ArcSelfReference;
    public union ArcCallChainTermValue(ArcFlexibleIdentifier, ArcFunctionCall, ArcSelfReference);

    public class ArcCallChainTerm: IArcTraceable<ArcSourceCodeParser.Arc_call_chain_termContext>
    {
        public ArcCallChainTerm(ArcSourceCodeParser.Arc_call_chain_termContext context)
        {
            if (context.arc_flexible_identifier() != null)
            {
                Value = new ArcFlexibleIdentifier(context.arc_flexible_identifier());
            }
            else if (context.arc_function_call_base() != null)
            {
                Value = new ArcFunctionCall(context.arc_function_call_base());
            }
            else if (context.arc_self_wrapper() != null)
            {
                Value = new ArcSelfReference();
            }
            else
            {
                throw new UnreachableException();
            }

            Indices = (context.arc_index() != null && context.arc_index().Length > 0) ?
                context.arc_index().Select(i => new ArcExpression(i.arc_expression())) :
                [];

            Context = context;
        }

        public ArcCallChainTermValue Value { get; }

        public IEnumerable<ArcExpression> Indices { get; set; }

        public ArcSourceCodeParser.Arc_call_chain_termContext Context { get; set; }
    }
}
