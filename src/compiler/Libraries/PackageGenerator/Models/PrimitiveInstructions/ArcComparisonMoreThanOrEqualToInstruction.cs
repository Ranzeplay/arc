using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcComparisonMoreThanOrEqualToInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x16];
    }
}
