using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class LogicalNotInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x0e];
    }
}
