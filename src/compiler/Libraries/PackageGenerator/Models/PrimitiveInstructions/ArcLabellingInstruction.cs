using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcLabellingInstruction(ArcRelocationLabelType labelType, string name, Guid relocationLayer) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x33];

        public ArcRelocationLabel Label { get; } = new()
        {
            Name = name,
            Type = labelType,
            Location = 0,
            Layer = relocationLayer
        };

        public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new()
            {
                GeneratedData = [.. Opcode],
                RelocationLabels = [Label]
            };
        }
    }
}
