using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcPopToSlotInstruction(ArcDataSlot slot) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode { get; } = [0x06];

        private ArcDataSlot Slot { get; } = slot;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = [.. Opcode, .. BitConverter.GetBytes(Slot.SlotId)],
            };
        }
    }
}
