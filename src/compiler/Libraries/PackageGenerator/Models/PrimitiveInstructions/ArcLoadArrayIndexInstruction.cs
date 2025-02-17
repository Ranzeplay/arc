using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    class ArcLoadArrayIndexInstruction(int dimension) : ArcPrimitiveInstructionBase
    {
        public int Dimension { get; set; } = dimension;

        public override byte[] Opcode => [0x43];

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = [.. Opcode, .. BitConverter.GetBytes(Dimension)],
            };
        }
    }
}
