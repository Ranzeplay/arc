using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcLogicalAndInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x0d];
    }
}
