using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant
{
    internal class ArcInstantValue
    {
        public enum ValueType
        {
            String,
            Integer,
            Decimal,
            Boolean
        }

        public ValueType Type { get; set; }

        public ArcBooleanValue? BooleanValue { get; set; }

        public ArcInstantStringValue? StringValue { get; set; }

        public ArcInstantIntegerValue? IntegerValue { get; set; }

        public ArcInstantDecimalValue? DecimalValue { get; set; }

        public ArcInstantValue(ArcInstantStringValue stringValue)
        {
            Type = ValueType.String;
            StringValue = stringValue;
        }

        public ArcInstantValue(ArcInstantIntegerValue integerValue)
        {
            Type = ValueType.Integer;
            IntegerValue = integerValue;
        }

        public ArcInstantValue(ArcInstantDecimalValue decimalValue)
        {
            Type = ValueType.Decimal;
            DecimalValue = decimalValue;
        }

        public ArcInstantValue(ArcBooleanValue booleanValue)
        {
            Type = ValueType.Boolean;
            BooleanValue = booleanValue;
        }

        public static ArcInstantValue FromTokens(ArcSourceCodeParser.Arc_instant_valueContext context)
        {
            if (context.NUMBER() != null)
            {
                var numberText = context.NUMBER().GetText();
                if (numberText.Contains('.'))
                {
                    return new ArcInstantValue(new ArcInstantDecimalValue(decimal.Parse(numberText)));
                }
                else
                {
                    return new ArcInstantValue(new ArcInstantIntegerValue(int.Parse(numberText)));
                }
            }
            else if (context.LITERAL_STRING() != null)
            {
                return new ArcInstantValue(new ArcInstantStringValue(context.LITERAL_STRING().GetText(), false));
            }
            else if (context.arc_bool_value() != null)
            {
                return new ArcInstantValue(new ArcBooleanValue(context.arc_bool_value().KW_TRUE() != null));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
