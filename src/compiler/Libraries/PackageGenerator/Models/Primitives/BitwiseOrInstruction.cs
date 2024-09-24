using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class BitwiseOrInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x10];
    }
}
