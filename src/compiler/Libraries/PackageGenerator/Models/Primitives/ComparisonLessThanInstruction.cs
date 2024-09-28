using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ComparisonLessThanInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x17];
    }
}
