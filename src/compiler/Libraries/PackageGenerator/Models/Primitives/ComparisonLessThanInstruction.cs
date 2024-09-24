using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ComparisonLessThanInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x17];
    }
}
