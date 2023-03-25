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
            var number = IntegerPart.ToByteArray().ToList();
            if(number.Count % metadata.DataSectionSize > 0) {
                number.InsertRange(0, new byte[number.Count % metadata.DataSectionSize]);
            }

            result.Add((byte)(number.Count / metadata.DataSectionSize));
            result.AddRange(number);

            // Add decimal point position
            var decimalPointPosition = DecimalPart.ToByteArray().ToList();
            if (decimalPointPosition.Count % metadata.DataSectionSize > 0)
            {
                decimalPointPosition.InsertRange(0, new byte[decimalPointPosition.Count % metadata.DataSectionSize]);
            }

            result.Add((byte)(decimalPointPosition.Count / metadata.DataSectionSize));
            result.AddRange(decimalPointPosition);

            return result.ToArray();
        }
    }
}
