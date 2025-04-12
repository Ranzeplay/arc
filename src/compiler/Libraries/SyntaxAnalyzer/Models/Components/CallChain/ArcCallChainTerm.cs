using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;
using System.Diagnostics;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain
{
    public class ArcCallChainTerm
    {
        public ArcCallChainTerm(ArcSourceCodeParser.Arc_call_chain_termContext context)
        {
            if (context.arc_flexible_identifier() != null)
            {
                Type = ArcCallChainTermType.Identifier;
                Identifier = new ArcFlexibleIdentifier(context.arc_flexible_identifier());
            }
            else if (context.arc_function_call_base() != null)
            {
                Type = ArcCallChainTermType.FunctionCall;
                FunctionCall = new ArcFunctionCall(context.arc_function_call_base());
            }
            else if (context.arc_self_wrapper() != null)
            {
                Type = ArcCallChainTermType.Identifier;
                Identifier = new ArcFlexibleIdentifier(new ArcSingleIdentifier(context.arc_self_wrapper()));
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

        public ArcCallChainTermType Type { get; set; }

        public ArcFlexibleIdentifier? Identifier { get; set; }

        public ArcFunctionCall? FunctionCall { get; set; }

        public IEnumerable<ArcExpression> Indices { get; set; }

        public ArcSourceCodeParser.Arc_call_chain_termContext Context { get; set; }
    }
}
