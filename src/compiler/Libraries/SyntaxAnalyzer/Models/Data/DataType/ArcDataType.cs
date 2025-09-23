using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    public class ArcDataType : IArcTraceable<ArcSourceCodeParser.Arc_data_typeContext>, IArcLocatable
    {
        public ArcDataType(ArcSourceCodeParser.Arc_data_typeContext context)
        {
            Dimension = context.arc_array_indicator().Length;
            DataType = context.arc_primitive_data_type() != null ? DataMemberType.Primitive : DataMemberType.Complex;
            if (DataType == DataMemberType.Primitive)
            {
                PrimitiveType = ArcPrimitiveDataTypeUtils.FromToken(context.arc_primitive_data_type());
            }
            else
            {
                ComplexType = new ArcComplexDataType(context.arc_flexible_identifier());
            }

            SpecializedGenericTypes = context.arc_generic_specialization_wrapper()?
                .arc_data_type()
                .Select(g => new ArcDataType(g)) ?? [];

            Context = context;
        }

        public DataMemberType DataType { get; set; }

        public enum DataMemberType
        {
            Primitive,
            Complex
        }

        public ArcPrimitiveDataType? PrimitiveType { get; set; }

        public ArcComplexDataType? ComplexType { get; set; }

        public int Dimension { get; set; }

        public IEnumerable<ArcDataType> SpecializedGenericTypes { get; set; }

        public ArcSourceCodeParser.Arc_data_typeContext Context { get; }

        public string TypeName => DataType switch
        {
            DataMemberType.Primitive => PrimitiveType?.GetTypeName() ?? string.Empty,
            DataMemberType.Complex => ComplexType?.Identifier.ToString() ?? string.Empty,
            _ => string.Empty
        };

        public override string ToString() => $"{(Dimension > 0 ? "A" : "S")}{TypeName}";

        public string GetSignature() => ToString();
    }
}
