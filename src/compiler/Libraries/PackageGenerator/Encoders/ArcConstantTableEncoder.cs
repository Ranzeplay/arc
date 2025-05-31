using Arc.Compiler.PackageGenerator.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcConstantTableEncoder
    {
        public static IEnumerable<byte> Encode(ArcGeneratorContext context)
        {
            var result = new List<byte>();
            result.AddRange(BitConverter.GetBytes((long)context.Constants.Count));

            foreach (var constant in context.Constants)
            {
                var encodedValue = constant.Encode();
                result.AddRange(encodedValue);

                context.Logger.LogTrace("Constant: {}", BitConverter.ToString([.. encodedValue]).Replace("-", " "));
            }

            context.Logger.LogInformation("Generated {} constants into constant table", context.Constants.Count);

            return result;
        }
    }
}
