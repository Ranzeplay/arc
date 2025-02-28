using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System.ComponentModel;
using System.Text;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant
{
    public class ArcInstantValue
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
                    return new ArcInstantValue(new ArcInstantIntegerValue((long)new Int64Converter().ConvertFromString(numberText)));
                }
            }
            else if (context.LITERAL_STRING() != null)
            {
                var text = context.LITERAL_STRING().GetText()[1..^1];
                // Handle escape sequences
                text = ConvertToEscapedString(text);

                return new ArcInstantValue(new ArcInstantStringValue(text, false));
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

        public string TypeName => Type switch
        {
            ValueType.String => "string",
            ValueType.Integer => "int",
            ValueType.Decimal => "decimal",
            ValueType.Boolean => "bool",
            _ => throw new NotImplementedException(),
        };

        public object GetRawValue()
        {
            return Type switch
            {
                ValueType.String => StringValue!.Value,
                ValueType.Integer => IntegerValue!.Value,
                ValueType.Decimal => DecimalValue!.Value,
                ValueType.Boolean => BooleanValue!.Value,
                _ => throw new InvalidCastException(),
            };
        }

        public static string ConvertToEscapedString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                // Check for escape sequence
                if (i < input.Length - 1 && input[i] == '\\')
                {
                    char nextChar = input[i + 1];
                    switch (nextChar)
                    {
                        case 'n':
                            result.Append('\n');
                            break;
                        case 'r':
                            result.Append('\r');
                            break;
                        case 't':
                            result.Append('\t');
                            break;
                        case '\\':
                            result.Append('\\');
                            break;
                        case '"':
                            result.Append('"');
                            break;
                        case '\'':
                            result.Append('\'');
                            break;
                        case 'b':
                            result.Append('\b');
                            break;
                        case 'f':
                            result.Append('\f');
                            break;
                        case 'u':
                            // Handle Unicode escape sequences \uXXXX
                            if (i + 5 < input.Length)
                            {
                                string hex = input.Substring(i + 2, 4);
                                if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int unicode))
                                {
                                    result.Append((char)unicode);
                                    i += 5; // Skip the unicode sequence
                                    continue;
                                }
                            }
                            result.Append(nextChar);
                            break;
                        default:
                            // If it's not a recognized escape sequence, keep the backslash and the character
                            result.Append('\\');
                            result.Append(nextChar);
                            break;
                    }
                    i++; // Skip the next character since we've already processed it
                }
                else
                {
                    result.Append(input[i]);
                }
            }

            return result.ToString();
        }
    }
}
