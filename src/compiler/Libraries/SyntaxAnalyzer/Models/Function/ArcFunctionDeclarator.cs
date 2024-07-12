using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    internal class ArcFunctionDeclarator(ArcSourceCodeParser.Arc_function_declaratorContext context)
    {
        public IEnumerable<ArcAnnotation> Annotations { get; set; } = context.arc_annotation().Select(a => new ArcAnnotation(a));

        public ArcAccessibility Accessibility { get; set; } = ArcAccessibilityUtils.FromToken(context.arc_accessibility());

        public ArcSingleIdentifier Identifier { get; set; } = new ArcSingleIdentifier(context.arc_single_identifier());

        public IEnumerable<ArcFunctionArgument> Arguments { get; set; } = context.arc_wrapped_arg_list().arc_arg_list().arc_data_declarator().Select(p => new ArcFunctionArgument(p));
    }
}
