using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    class ArcReplaceStackTopInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x41];
    }
}
