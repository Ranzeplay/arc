using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group
{
    internal class ArcGroupField
    {
        public IEnumerable<ArcAnnotation> Annotations { get; set; }

        public ArcAccessibility Accessibility { get; set; }

        public ArcDataDeclarator DataDeclarator { get; set; }

        public ArcGroupField(ArcSourceCodeParser.Arc_group_fieldContext context)
        {
            Annotations = context.arc_annotation().Select(a => new ArcAnnotation(a));
            Accessibility = ArcAccessibilityUtils.FromToken(context.arc_accessibility());
            DataDeclarator = new(context.arc_data_declarator());
        }
    }
}
