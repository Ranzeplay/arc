using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ComparisonValueNotEqualInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x31];
    }
}
