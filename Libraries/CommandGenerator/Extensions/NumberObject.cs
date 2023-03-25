using Arc.Compiler.Shared.CommandGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.CompilerCommandGenerator.Extensions
{
    internal class NumberObject : Compiler.Shared.Math.NumberObject
    {
        public NumberObject(string number) : base(number)
        {
        }

        /// <summary>
        /// Output format: <Sign> <DS | Integer part> <DS | Decimal part>
        /// </summary>
        /// <param name="metadata">A package metadata</param>
        /// <returns>The byte array built</returns>
        public byte[] ToPackageEncoding(PackageMetadata metadata)
        {
            var result = new List<byte>
            {
                // Positive/negative symbol
                (byte)(IsPositive ? 1 : 0)
            };

            // Build numbers
            var integerPart = Utils.BuildDataBlock(IntegerPart.ToByteArray().ToList(), metadata);
            var decimalPart = Utils.BuildDataBlock(DecimalPart.ToByteArray().ToList(), metadata);

            result.AddRange(integerPart);
            result.AddRange(decimalPart);

            return result.ToArray();
        }
    }
}
