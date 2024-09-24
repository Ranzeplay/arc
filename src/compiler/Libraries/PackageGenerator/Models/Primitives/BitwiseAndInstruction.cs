using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class BitwiseAndInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x0f];
    }
}
