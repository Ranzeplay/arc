using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcConditionalJumpInstruction(ArcRelocationTarget target) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x22];

        public ArcRelocationTarget Target { get; set; } = target;

        public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            Target.Location = 1;
            return new ArcPartialGenerationResult
            {
                GeneratedData = [.. Opcode, .. BitConverter.GetBytes((long)0)],
                RelocationTargets = [
                    Target
                ],
            };
        }
    }
}
