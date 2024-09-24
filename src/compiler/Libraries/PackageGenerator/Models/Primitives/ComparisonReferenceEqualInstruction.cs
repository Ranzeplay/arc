using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ComparisonReferenceEqualInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x13];
    }
}
