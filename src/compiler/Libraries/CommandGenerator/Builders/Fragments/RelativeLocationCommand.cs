using Arc.Compiler.CommandGenerator.Models;
using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Compiler.CommandGenerator.Builders.Fragments
{
    internal class RelativeLocationCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<long> source)
        {
            var commands = new List<byte>
            {
                (byte)(source.Component >= 0 ? 0x0F : 0x0B)
            };

            var absLocation = PackageMetadata.GenerateDataAligned(long.Abs(source.Component), source.PackageMetadata.AddressAlignment);
            commands.AddRange(absLocation);

            return new PartialGenerationResult(commands);
        }
    }
}
