using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class UnconditionalJumpInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x21];

        public ArcGenerationResult Encode(ArcGenerationSource<(ArcRelocationType, long)> source)
        {
            return new ArcGenerationResult
            {
                GeneratedData = Opcode.Concat(BitConverter.GetBytes((long)0)),
                RelocationDescriptors = [
                    new()
                    {
                        Id = new Random().Next(),
                        Type = source.Value.Item1,
                        CommandBeginLocation = 1,
                        Location = source.Value.Item2,
                    },
                ],
            };
        }

        public ArcGenerationResult Encode(ArcGenerationSource<ArcLabel> source)
        {
            return new ArcGenerationResult
            {
                GeneratedData = Opcode.Concat(BitConverter.GetBytes((long)0)),
                RelocationDescriptors = [
                    new()
                    {
                        Id = 0,
                        Type = ArcRelocationType.Symbol,
                        CommandBeginLocation = 1,
                        Location = source.Value.Position,
                    },
                ],
            };
        }
    }
}
