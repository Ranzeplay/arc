using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    internal class ArcDataType
    {
        public ArcDataType(ArcSourceCodeParser.Arc_data_typeContext context)
        {
            IsArray = context.arc_array_data_flag() != null;
            DataType = context.arc_primitive_data_type() != null ? DataMemberType.Primitive : DataMemberType.Derivative;
            if (DataType == DataMemberType.Primitive)
            {
                PrimitiveType = ArcPrimitiveDataTypeUtils.FromToken(context.arc_primitive_data_type());
            }
            else
            {
                DerivativeType = new ArcDerivativeDataType(context.arc_derivative_data_type());
            }
        }

        public DataMemberType DataType { get; set; }

        public enum DataMemberType
        {
            Primitive,
            Derivative
        }

        public ArcPrimitiveDataType? PrimitiveType { get; set; }

        public ArcDerivativeDataType? DerivativeType { get; set; }

        public bool IsArray { get; set; }
    }
}
