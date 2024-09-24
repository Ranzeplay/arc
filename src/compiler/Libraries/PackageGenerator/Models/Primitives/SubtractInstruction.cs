using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class SubtractInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x08];
    }
}
