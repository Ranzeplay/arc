using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class BitwiseNotInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x11];
    }
}
