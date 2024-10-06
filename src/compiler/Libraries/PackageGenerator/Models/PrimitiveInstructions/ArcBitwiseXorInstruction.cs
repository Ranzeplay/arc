using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcBitwiseXorInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x34];
    }
}
