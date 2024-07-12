using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    internal enum ArcParameterType
    {
        Reference,
        Value
    }

    internal static class ArcParameterTypeUtils
    {
        public static ArcParameterType FromToken(ArcSourceCodeParser.Arc_param_typeContext context)
        {
            if(context.KEYWORD_REF != null) return ArcParameterType.Reference;
            if (context.KEYWORD_VAL != null) return ArcParameterType.Value;
            throw new InvalidConstraintException("Invalid parameter type");
        }
    }
}
