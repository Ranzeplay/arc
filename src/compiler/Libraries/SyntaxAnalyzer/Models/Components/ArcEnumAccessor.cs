using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    public class ArcEnumAccessor(ArcSourceCodeParser.Arc_enum_accessorContext context)
    {
        public ArcFlexibleIdentifier Name { get; set; } = new ArcFlexibleIdentifier(context.arc_flexible_identifier());

        public ArcSingleIdentifier Member { get; set; } = new ArcSingleIdentifier(context.arc_single_identifier());

        public ArcSourceCodeParser.Arc_enum_accessorContext Context { get; } = context;
    }
}
