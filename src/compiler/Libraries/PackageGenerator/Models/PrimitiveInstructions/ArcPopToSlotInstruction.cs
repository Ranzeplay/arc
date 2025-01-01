using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Relocation;

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
                GeneratedData = [.. Opcode, .. BitConverter.GetBytes(slot.SlotId)],
                RelocationTargets = [
                    new()
                    {
                        Location = 1,
                        TargetType = ArcRelocationTargetType.Symbol,
                        Symbol = Slot
                    }
                ]
            };
        }
    }
}
