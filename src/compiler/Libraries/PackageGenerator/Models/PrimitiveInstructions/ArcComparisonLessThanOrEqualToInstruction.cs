using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcComparisonLessThanOrEqualToInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x18];
    }
}
