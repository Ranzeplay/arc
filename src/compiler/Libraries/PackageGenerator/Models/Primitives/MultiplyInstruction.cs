using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class MultiplyInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x08];
    }
}
