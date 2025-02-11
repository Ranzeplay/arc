using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcInstantValueEncoder
    {
        public static IArcConstantEncoder GetEncoderFromInstantValue(ArcInstantValue value)
        {
            return value.Type switch
            {
                ArcInstantValue.ValueType.Integer => new ArcIntegerEncoder(),
                ArcInstantValue.ValueType.Decimal => new ArcDecimalEncoder(),
                ArcInstantValue.ValueType.String => new ArcStringEncoder(),
                ArcInstantValue.ValueType.Boolean => new ArcBooleanEncoder(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
