using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    internal enum ArcAccessibility
    {
        Public,
        Internal,
        Protected,
        Private
    }

    internal static class ArcAccessibilityUtils
    {
        public static ArcAccessibility FromToken(ArcSourceCodeParser.Arc_accessibilityContext context)
        {
            if (context.KEYWORD_PUBLIC() != null) return ArcAccessibility.Public;
            if (context.KEYWORD_INTERNAL() != null) return ArcAccessibility.Internal;
            if (context.KEYWORD_PROTECTED() != null) return ArcAccessibility.Protected;
            if (context.KEYWORD_PRIVATE() != null) return ArcAccessibility.Private;
            throw new InvalidConstraintException("Invalid accessibility token");
        }
    }
}
