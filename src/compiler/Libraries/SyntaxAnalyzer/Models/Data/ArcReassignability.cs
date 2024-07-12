using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data
{
    internal enum ArcReassignability
    {
        Constant,
        Variable
    }

    internal static class ArcReassignabilityUtils
    {
        public static ArcReassignability FromToken(ArcSourceCodeParser.Arc_reassignabilityContext context)
        {
            if (context.KEYWORD_CONST() != null) return ArcReassignability.Constant;
            if (context.KEYWORD_VAR() != null) return ArcReassignability.Variable;
            throw new InvalidConstraintException("Invalid reassignability type");
        }
    }
}
