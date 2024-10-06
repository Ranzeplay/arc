using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class BitwiseAndInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x0f];
    }
}
