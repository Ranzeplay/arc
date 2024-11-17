using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group
{
    public class ArcGroupField : IArcTraceable<ArcSourceCodeParser.Arc_group_fieldContext>, IArcLocatable
    {
        public IEnumerable<ArcAnnotation> Annotations { get; set; }

        public ArcAccessibility Accessibility { get; set; }

        public ArcDataDeclarator DataDeclarator { get; set; }
        
        public ArcSourceCodeParser.Arc_group_fieldContext Context { get; }

        public ArcGroupField(ArcSourceCodeParser.Arc_group_fieldContext context)
        {
            Context = context;
            Annotations = context.arc_annotation().Select(a => new ArcAnnotation(a));
            Accessibility = ArcAccessibilityUtils.FromToken(context.arc_accessibility());
            DataDeclarator = new(context.arc_data_declarator());
        }

        public string GetSignature()
        {
            return $"D{DataDeclarator.GetSignature()}";
        }
    }
}
