using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    public union ArcDataTypeValue(ArcPrimitiveDataType, ArcComplexDataType);

    public class ArcDataType : IArcTraceable<ArcSourceCodeParser.Arc_data_typeContext>, IArcLocatable
    {
        public ArcDataType(ArcSourceCodeParser.Arc_data_typeContext context)
        {
            Dimension = context.arc_array_indicator().Length;
            Value = context.arc_primitive_data_type() is { } primitive
                ? ArcPrimitiveDataTypeUtils.FromToken(primitive)
                : new ArcComplexDataType(context.arc_flexible_identifier());

            SpecializedGenericTypes = context.arc_generic_specialization_wrapper()?
                .arc_data_type()
                .Select(g => new ArcDataType(g)) ?? [];

            Context = context;
        }

        public ArcDataTypeValue Value { get; }

        public int Dimension { get; set; }

        public IEnumerable<ArcDataType> SpecializedGenericTypes { get; set; }

        public ArcSourceCodeParser.Arc_data_typeContext Context { get; }

        public string TypeName => Value switch
        {
            ArcPrimitiveDataType primitive => primitive.GetTypeName(),
            ArcComplexDataType complex => complex.Identifier.ToString(),
            null => throw new InvalidOperationException("Uninitialized data type"),
        };

        public override string ToString() => $"{(Dimension > 0 ? "A" : "S")}{TypeName}";

        public string GetSignature() => ToString();
    }
}
