using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    public class ArcFunctionDeclarator : IArcLocatable
    {
        public IEnumerable<ArcAnnotation> Annotations { get; set; }

        public ArcAccessibility Accessibility { get; set; }

        public ArcSingleIdentifier Identifier { get; set; }

        public IEnumerable<ArcFunctionArgument> Arguments { get; set; }

        public ArcDataType ReturnType { get; set; }

        public ArcFunctionDeclarator(ArcSourceCodeParser.Arc_function_declaratorContext context, bool allowSelf)
        {

            Annotations = context.arc_annotation().Select(a => new ArcAnnotation(a));
            Accessibility = ArcAccessibilityUtils.FromToken(context.arc_accessibility());
            Identifier = new(context.arc_single_identifier());
            ReturnType = new(context.arc_data_type());

            if (context.arc_wrapped_arg_list()?.arc_arg_list().arc_self_data_declarator() != null)
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
