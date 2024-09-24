using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ComparisonMoreThanInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x15];
    }
}
