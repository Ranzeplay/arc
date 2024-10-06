using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcLogicalNotInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x0e];
    }
}
