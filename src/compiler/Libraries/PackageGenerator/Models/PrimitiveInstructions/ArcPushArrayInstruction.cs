using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    class ArcPushArrayInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x45];
    }
}
