using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    internal class ArcFunctionCall(ArcSourceCodeParser.Arc_function_callContext context)
    {
        public ArcScopedIdentifier Identifier { get; set; } = new ArcScopedIdentifier(context.arc_scoped_identifier());

        public IEnumerable<ArcFunctionCallArgument> Arguments { get; set; } = context.arc_call_args().arc_expression().Select(arg => new ArcFunctionCallArgument(arg));
    }
}
