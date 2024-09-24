using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ComparisonReferenceNotEqualInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x32];
    }
}
