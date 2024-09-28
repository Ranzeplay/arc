using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class LogicalOrInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x0f];
    }
}
