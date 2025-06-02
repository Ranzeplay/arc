using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    public class ArcEnumMember(ArcSourceCodeParser.Arc_enum_memberContext context)
    {
        public ArcSingleIdentifier Name { get; set; } = new ArcSingleIdentifier(context.arc_single_identifier());

        public IEnumerable<ArcAnnotation> Annotations { get; set; } = context.arc_annotation().Select(a => new ArcAnnotation(a));

        public ArcSourceCodeParser.Arc_enum_memberContext Context { get; } = context;
    }
}
