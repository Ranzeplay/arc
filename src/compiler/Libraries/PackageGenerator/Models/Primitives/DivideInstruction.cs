using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class DivideInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x0a];
    }
}
