using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    class ArcRemoveArrayIndexInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x44];
    }
}
