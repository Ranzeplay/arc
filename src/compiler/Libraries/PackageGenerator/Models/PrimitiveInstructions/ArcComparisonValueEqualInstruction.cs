using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcComparisonValueEqualInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x13];
    }
}
