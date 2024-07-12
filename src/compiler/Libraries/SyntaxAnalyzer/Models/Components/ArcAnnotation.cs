using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    internal class ArcAnnotation(ArcSourceCodeParser.Arc_annotationContext context)
    {
        public ArcFlexibleIdentifier Identifier { get; set; } = new ArcFlexibleIdentifier(context.arc_flexible_identifier());

        public IEnumerable<ArcFunctionCallArgument> CallArguments { get; set; } = context.arc_wrapped_param_list().arc_param_list().arc_expression().Select(arg => new ArcFunctionCallArgument(arg));
    }
}
