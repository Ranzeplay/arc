using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    public class ArcNamedFunctionDeclarator : ArcFunctionMinimalDeclarator, IArcLocatable, IArcAccessible
    {
        public IEnumerable<ArcAnnotation> Annotations { get; set; }

        public ArcSingleIdentifier Identifier { get; set; }
        
        public ArcAccessibility Accessibility { get; set; }

        public ArcNamedFunctionDeclarator(ArcSourceCodeParser.Arc_function_declaratorContext context, bool allowSelf)
        {

            Annotations = context.arc_annotation().Select(a => new ArcAnnotation(a));
            Accessibility = ArcAccessibilityUtils.FromToken(context.arc_accessibility());
            Identifier = new(context.arc_single_identifier());
            ReturnType = new(context.arc_data_type());
            GenericTypes = context.arc_generic_declaration_wrapper()?.arc_single_identifier().Select(g => new ArcSingleIdentifier(g)) ?? [];

            if (context.arc_wrapped_arg_list().arc_arg_list()?.arc_self_data_declarator() != null)
            {
                if (allowSelf)
                {
                    Arguments = [
                        new ArcFunctionArgument(context.arc_wrapped_arg_list().arc_arg_list().arc_self_data_declarator()),
                        .. context.arc_wrapped_arg_list().arc_arg_list().arc_data_declarator().Select(p => new ArcFunctionArgument(p))
                        ];
                }
                else
                {
                    throw new InvalidDataException("Self argument is not allowed in this context");
                }
            }
            else
            {
                Arguments = context.arc_wrapped_arg_list().arc_arg_list()?.arc_data_declarator().Select(p => new ArcFunctionArgument(p)) ?? [];
            }
        }

        public string GetSignature() => $"F{Identifier}@{string.Join('&', Arguments.Select(a => a.DataType.GetSignature()))}*{ReturnType.GetSignature()}";
    }
}
