using Arc.Compiler.PackageGenerator.Interfaces;
namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ComparisonMoreThanOrEqualToInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x16];
    }
}
