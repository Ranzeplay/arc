using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data
{
    internal class ArcDataValue
    {
        public enum ValueType
        {
            InstantValue,
            FunctionCall,
            FlexibleIdentifier,
            TypeValue,
            None
        }

        public ValueType Type { get; set; }

        public ArcInstantValue? InstantValue { get; set; }

        public ArcFunctionCall? FunctionCall { get; set; }

        public ArcFlexibleIdentifier? FlexibleIdentifier { get; set; }

        public ArcDataType? TypeValue { get; set; }

        public ArcDataValue(ArcInstantValue value)
        {
            Type = ValueType.InstantValue;
            InstantValue = value;
        }

        public ArcDataValue(ArcFunctionCall function)
        {
            Type = ValueType.FunctionCall;
            FunctionCall = function;
        }

        public ArcDataValue(ArcFlexibleIdentifier id)
        {
            Type = ValueType.FlexibleIdentifier;
            FlexibleIdentifier = id;
        }

        public ArcDataValue(ArcDataType type)
        {
            Type = ValueType.TypeValue;
            TypeValue = type;
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
            else if (context.arc_function_call_base() != null)
            {
                return new ArcDataValue(new ArcFunctionCall(context.arc_function_call_base()));
            }
            else if (context.arc_flexible_identifier() != null)
            {
                return new ArcDataValue(new ArcFlexibleIdentifier(context.arc_flexible_identifier()));
            }
            else if (context.arc_type_value() != null)
            {
                return new ArcDataValue(new ArcDataType(context.arc_type_value().arc_data_type()));
            }
            else
            {
                return new ArcDataValue();
            }
        }
    }
}
