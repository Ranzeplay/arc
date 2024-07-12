using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data
{
    internal enum ArcMutability
    {
        Constant,
        Variable
    }

    internal static class ArcMutabilityUtils
    {
        public static ArcMutability FromToken(ArcSourceCodeParser.Arc_mutabilityContext context)
        {
            if (context.KW_CONSTANT() != null) return ArcMutability.Constant;
            if (context.KW_VARIABLE() != null) return ArcMutability.Variable;
            throw new InvalidConstraintException("Invalid mutability type");
        }
    }
}
