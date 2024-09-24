using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class AddInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x07];
    }
}
