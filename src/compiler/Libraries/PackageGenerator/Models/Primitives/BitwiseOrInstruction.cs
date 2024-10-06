using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class BitwiseOrInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x10];
    }
}
