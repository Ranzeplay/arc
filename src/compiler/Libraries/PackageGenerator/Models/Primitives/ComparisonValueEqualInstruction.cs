using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ComparisonValueEqualInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x13];
    }
}
