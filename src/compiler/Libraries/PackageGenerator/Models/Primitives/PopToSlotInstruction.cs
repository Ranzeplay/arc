using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class PopToSlotInstruction(ArcDataSlot slot) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode { get; } = [0x06];

        private ArcDataSlot Slot { get; } = slot;

        public new ArcGenerationResult Encode<T>(ArcGenerationSource<T> source)
        {
            return new ArcGenerationResult
            {
                GeneratedData = Opcode
                    .Concat(BitConverter.GetBytes(slot.SlotId)),
                RelocationDescriptors = [
                    new()
                    {
                        Id = new Random().Next(),
                        CommandBeginLocation = 1,
                        Type = ArcRelocationType.Symbol,
                        Target = new(Slot)
                    }
                ]
            };
        }
    }
}
