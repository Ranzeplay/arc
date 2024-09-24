using Arc.Compiler.PackageGenerator.Interfaces;
namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ComparisonLessThanOrEqualToInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x18];
    }
}
