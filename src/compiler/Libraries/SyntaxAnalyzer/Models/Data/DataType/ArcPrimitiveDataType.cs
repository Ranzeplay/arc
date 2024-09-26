using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    public enum ArcPrimitiveDataType
    {
        Integer,
        Decimal,
        Char,
        String,
        Bool,
        None,
        Any,
        Infer
    }

    public static class ArcPrimitiveDataTypeUtils
    {
        public static ArcPrimitiveDataType FromToken(ArcSourceCodeParser.Arc_primitive_data_typeContext context)
        {
            if (context.KW_INFER() != null) return ArcPrimitiveDataType.Infer;
            if (context.KW_BOOL() != null) return ArcPrimitiveDataType.Bool;
            if (context.KW_CHAR() != null) return ArcPrimitiveDataType.Char;
            if (context.KW_INT() != null) return ArcPrimitiveDataType.Integer;
            if (context.KW_DECIMAL() != null) return ArcPrimitiveDataType.Decimal;
            if (context.KW_STRING() != null) return ArcPrimitiveDataType.String;
            if (context.KW_ANY() != null) return ArcPrimitiveDataType.Any;
            if (context.KW_NONE() != null) return ArcPrimitiveDataType.None;
            throw new InvalidConstraintException("Invalid primitive data type");
        }

        public static string GetTypeName(this ArcPrimitiveDataType type)
        {
            return type switch
            {
                ArcPrimitiveDataType.Infer => "infer",
                ArcPrimitiveDataType.Bool => "bool",
                ArcPrimitiveDataType.Char => "char",
                ArcPrimitiveDataType.Integer => "int",
                ArcPrimitiveDataType.Decimal => "decimal",
                ArcPrimitiveDataType.String => "str",
                ArcPrimitiveDataType.Any => "any",
                ArcPrimitiveDataType.None => "none",
                _ => throw new InvalidConstraintException("Invalid primitive data type")
            };
        }
    }
}
