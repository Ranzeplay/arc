using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    internal enum ArcPrimitiveDataType
    {
        Number,
        Char,
        String,
        Bool,
        None,
        Any,
        Auto
    }

    internal static class ArcPrimitiveDataTypeUtils
    {
        public static ArcPrimitiveDataType FromToken(ArcSourceCodeParser.Arc_primitive_data_typeContext context)
        {
            if (context.KEYWORD_AUTO() != null) return ArcPrimitiveDataType.Auto;
            if (context.KEYWORD_BOOL() != null) return ArcPrimitiveDataType.Bool;
            if (context.KEYWORD_CHAR() != null) return ArcPrimitiveDataType.Char;
            if (context.KEYWORD_NUMBER() != null) return ArcPrimitiveDataType.Number;
            if (context.KEYWORD_STRING() != null) return ArcPrimitiveDataType.String;
            if (context.KEYWORD_ANY() != null) return ArcPrimitiveDataType.Any;
            if (context.KEYWORD_NONE() != null) return ArcPrimitiveDataType.None;
            throw new InvalidConstraintException("Unknown primitive data type");
        }
    }
}
