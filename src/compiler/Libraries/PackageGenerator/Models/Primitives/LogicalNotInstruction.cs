using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class LogicalNotInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x0e];
    }
}
