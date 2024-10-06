using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class UnconditionalJumpInstruction : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x21];

        public required ArcRelocationTarget Target { get; set; }

        public ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = Opcode.Concat(BitConverter.GetBytes((long)0)),
                RelocationTargets = [
                    Target
                ],
            };
        }
    }
}
