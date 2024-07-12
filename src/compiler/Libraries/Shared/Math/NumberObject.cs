using System.Numerics;

namespace Arc.Compiler.Shared.Math
{
    public class NumberObject
    {
        public bool IsPositive { get; set; }

        public BigInteger IntegerPart { get; set; }

        public BigInteger DecimalPart { get; set; }

        public int DecimalPrecision { get; }

        public NumberObject(bool isPositive, BigInteger integerPart, BigInteger decimalPart)
        {
            IsPositive = isPositive;
            IntegerPart = integerPart;
            DecimalPart = decimalPart;
            DecimalPrecision = decimalPart.ToString().Length;
        }

        public NumberObject(bool isPositive, BigInteger integerPart)
        {
            IsPositive = isPositive;
            IntegerPart = integerPart;
            DecimalPart = 0;
            DecimalPrecision = 0;
        }

        public NumberObject(string number)
        {
            if (number[0] == '-')
            {
                IsPositive = false;

                // Remove the first symbol character
                number = number[1..];
            }
            else if (number[0] == '+')
            {
                IsPositive = true;
                number = number[1..];
            }
            else
            {
                IsPositive = true;
            }

            var splitByDecimalPoint = number.Split('.');
            IntegerPart = BigInteger.Parse(splitByDecimalPoint[0]);

            // Check if there's decimal part
            if(splitByDecimalPoint.Length > 1)
            {
                DecimalPart = BigInteger.Parse(splitByDecimalPoint[1]);
                DecimalPrecision = splitByDecimalPoint[1].Length;
            }
            else
            {
                DecimalPrecision = 0;
            }
        }

        /// <summary>
        /// Create number zero
        /// </summary>
        public NumberObject()
        {
            IsPositive = false;
            IntegerPart = 0;
            DecimalPart = 0;
            DecimalPrecision = 0;
        }

        public override string ToString()
        {
            string result = string.Empty;
            if (!IsPositive)
            {
                result += '-';
            }
            
            result += IntegerPart.ToString();
            if (DecimalPart != 0)
            {
                result += ".";
                result += DecimalPart.ToString();
            }
            else
            {
                if(DecimalPrecision > 0)
                {
                    result += ".";
                    result += new string('0', DecimalPrecision);
                }
            }

            return result;
        }

        public string ToString(bool forcePositiveSymbol)
        {
            var result = ToString();
            if (IsPositive && forcePositiveSymbol)
            {
                result = result.Insert(0, "+");
            }

            return result;
        }

        public static NumberObject Zero { get; } = new();
    }
}
