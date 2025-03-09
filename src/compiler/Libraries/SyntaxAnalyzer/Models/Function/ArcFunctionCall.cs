using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    public class ArcFunctionCall(ArcSourceCodeParser.Arc_function_call_baseContext context)
    {
        public ArcFlexibleIdentifier Identifier { get; set; } = new(context.arc_flexible_identifier());

        public IEnumerable<ArcFunctionCallArgument> Arguments { get; set; } = context.arc_wrapped_param_list().arc_param_list()?.arc_expression().Select(arg => new ArcFunctionCallArgument(arg)) ?? [];

        public ArcSourceCodeParser.Arc_function_call_baseContext Context { get; set; } = context;
    }
}
