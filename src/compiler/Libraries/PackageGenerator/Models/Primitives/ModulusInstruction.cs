using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class ModulusInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x33];
    }
}
