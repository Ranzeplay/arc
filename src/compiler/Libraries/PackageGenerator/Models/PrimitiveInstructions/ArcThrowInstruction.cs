using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcThrowInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x1b];
    }
}
