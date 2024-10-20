using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    public class ArcDataType : IArcTraceable<ArcSourceCodeParser.Arc_data_typeContext>, IArcLocatable
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

            Context = context;
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

        public ArcSourceCodeParser.Arc_data_typeContext Context { get; }

        public string TypeName => DataType switch
        {
            DataMemberType.Primitive => PrimitiveType?.GetTypeName() ?? string.Empty,
            DataMemberType.Derivative => DerivativeType?.Identifier.ToString() ?? string.Empty,
            _ => string.Empty
        };

        public override string ToString() => $"{(MemoryStorageType == ArcMemoryStorageType.Reference ? 'R' : 'V')}{(IsArray ? "A" : "S")}{TypeName}";

        public string GetSignature() => ToString();
    }
}
