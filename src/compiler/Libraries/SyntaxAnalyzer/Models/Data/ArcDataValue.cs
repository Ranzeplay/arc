using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data
{
    public class ArcDataValue
    {
        public enum ValueType
        {
            InstantValue,
            TypeValue,
            CallChain,
            EnumAccessor,
            None
        }

        public ValueType Type { get; set; }

        public ArcInstantValue? InstantValue { get; set; }

        public ArcDataType? TypeValue { get; set; }

        public ArcCallChain? CallChain { get; set; }

        public ArcEnumAccessor? EnumAccessor { get; set; }

        public ArcDataValue(ArcInstantValue value)
        {
            Type = ValueType.InstantValue;
            InstantValue = value;
        }

        public ArcDataValue(ArcDataType type)
        {
            Type = ValueType.TypeValue;
            TypeValue = type;
        }

        public ArcDataValue(ArcCallChain chain)
        {
            Type = ValueType.CallChain;
            CallChain = chain;
        }

        public ArcDataValue(ArcEnumAccessor enumAccessor)
        {
            Type = ValueType.EnumAccessor;
            EnumAccessor = enumAccessor;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ArcDataValue"/> with <see cref="ValueType.None"/>.
        /// </summary>
        public ArcDataValue()
        {
            Type = ValueType.None;
        }

        public static ArcDataValue FromTokens(ArcSourceCodeParser.Arc_data_valueContext context)
        {
            if (context.arc_instant_value() != null)
            {
                return new ArcDataValue(ArcInstantValue.FromTokens(context.arc_instant_value()));
            }
            else if (context.arc_type_value() != null)
            {
                return new ArcDataValue(new ArcDataType(context.arc_type_value().arc_data_type()));
            }
            else if (context.arc_call_chain() != null)
            {
                return new ArcDataValue(new ArcCallChain(context.arc_call_chain()));
            }
            else if (context.arc_enum_accessor() != null)
            {
                return new ArcDataValue(new ArcEnumAccessor(context.arc_enum_accessor()));
            }
            else
            {
                return new ArcDataValue();
            }
        }
    }
}
