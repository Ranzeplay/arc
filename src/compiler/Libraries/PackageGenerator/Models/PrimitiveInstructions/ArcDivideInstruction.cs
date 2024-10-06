using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcDivideInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x0a];
    }
}
