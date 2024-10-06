using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcBitwiseOrInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x10];
    }
}
