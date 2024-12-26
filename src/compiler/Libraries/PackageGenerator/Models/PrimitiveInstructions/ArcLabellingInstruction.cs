using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcLabellingInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x34];

        public ArcRelocationLabel Label { get; }

        public ArcLabellingInstruction(ArcRelocationLabelType labelType, string name)
        {
            Label = new()
            {
                Name = name,
                Type = labelType,
                Location = 0
            };
        }

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new()
            {
                GeneratedData = Opcode,
                RelocationLabels = [Label]
            };
        }
    }
}
