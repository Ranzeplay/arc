using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcDestroyIsolatedRegionInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x29];
    }
}
