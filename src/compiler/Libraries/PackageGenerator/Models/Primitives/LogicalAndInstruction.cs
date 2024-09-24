using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class LogicalAndInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x0d];
    }
}
