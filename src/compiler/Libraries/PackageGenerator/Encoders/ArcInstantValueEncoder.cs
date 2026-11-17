using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcInstantValueEncoder
    {
        public static IArcConstantEncoder GetEncoderFromInstantValue(ArcInstantValue value)
        {
            return value switch
            {
                ArcInstantIntegerValue => new ArcIntegerEncoder(),
                ArcInstantDecimalValue => new ArcDecimalEncoder(),
                ArcInstantStringValue => new ArcStringEncoder(),
                ArcBooleanValue => new ArcBooleanEncoder(),
                ArcAnyValue or ArcNoneValue => new ArcEmptyEncoder(),
                null => throw new InvalidOperationException("Uninitialized instant value"),
            };
        }
    }
}
