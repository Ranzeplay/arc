using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group
{
    internal class ArcGroupFunction : ArcFunctionBase<ArcSourceCodeParser.Arc_group_functionContext>
    {
        public ArcGroupFunction(ArcSourceCodeParser.Arc_group_functionContext context)
        {
            var func = context.arc_function_block();
            Declarator.Annotations = func.arc_function_declarator().arc_annotation().Select(a => new ArcAnnotation(a));
            Declarator.Accessibility = ArcAccessibilityUtils.FromToken(func.arc_function_declarator().arc_accessibility());
            Declarator.Identifier = new(func.arc_function_declarator().arc_single_identifier());
            Declarator.Arguments = func.arc_function_declarator().arc_wrapped_arg_list().arc_arg_list().arc_data_declarator().Select(p => new ArcFunctionArgument(p));

            Body = new(func.arc_wrapped_function_body());
            
            Context = context;
        }
    }
}
