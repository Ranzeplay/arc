using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    public class ArcBlockEnum(ArcSourceCodeParser.Arc_enum_declaratorContext context)
    {
        public ArcAccessibility Accessibility { get; set; } = ArcAccessibilityUtils.FromToken(context.arc_accessibility());

        public ArcSingleIdentifier Name { get; set; } = new ArcSingleIdentifier(context.arc_single_identifier());

        public IEnumerable<ArcAnnotation> Annotations { get; set; } = context.arc_annotation().Select(a => new ArcAnnotation(a));

        public IEnumerable<ArcEnumMember> Members { get; set; } = context.arc_enum_member().Select(m => new ArcEnumMember(m));

        public ArcSourceCodeParser.Arc_enum_declaratorContext Context { get; } = context;
    }
}
