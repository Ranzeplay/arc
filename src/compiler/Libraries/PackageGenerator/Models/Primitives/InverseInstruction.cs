using Arc.Compiler.PackageGenerator.Interfaces;
namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class InverseInstruction : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x12];
    }
}
