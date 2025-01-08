using Arc.Compiler.PackageGenerator.Models;

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

                Console.WriteLine(BitConverter.ToString(encodedValue.ToArray()).Replace("-", " "));
            }

            Console.WriteLine($"Constant table serialized, {context.Constants.Count} in total");

            return result;
        }
    }
}
