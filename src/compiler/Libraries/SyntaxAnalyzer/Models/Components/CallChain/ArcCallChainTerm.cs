using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
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
            else if (context.KW_SELF() != null)
            {
                Type = ArcCallChainTermType.Identifier;
                Identifier = new ArcFlexibleIdentifier(["self"]);
            }
            else
            {
                throw new UnreachableException();
            }
        }

        public ArcCallChainTermType Type { get; set; }

        public ArcFlexibleIdentifier? Identifier { get; set; }

        public ArcFunctionCall? FunctionCall { get; set; }
    }
}
