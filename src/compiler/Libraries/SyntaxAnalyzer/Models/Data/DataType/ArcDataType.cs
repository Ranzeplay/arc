using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    internal class ArcDataType
    {
        public ArcDataType(ArcSourceCodeParser.Arc_data_typeContext context)
        {
            IsArray = context.arc_array_indicator() != null;
            DataType = context.arc_primitive_data_type() != null ? DataMemberType.Primitive : DataMemberType.Derivative;
            MemoryStorageType = ArcMemoryStorageTypeUtils.FromToken(context.arc_mem_store_type());
            if (DataType == DataMemberType.Primitive)
            {
                PrimitiveType = ArcPrimitiveDataTypeUtils.FromToken(context.arc_primitive_data_type());
            }
            else
            {
                DerivativeType = new ArcDerivativeDataType(context.arc_flexible_identifier());
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

        public ArcMemoryStorageType MemoryStorageType { get; set; }

        public bool IsArray { get; set; }
    }
}
