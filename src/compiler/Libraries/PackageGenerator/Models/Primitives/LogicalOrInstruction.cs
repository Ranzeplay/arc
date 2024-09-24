using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class LogicalOrInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x0f];
    }
}
