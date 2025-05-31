using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcMultiplyInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x09];
    }
}
