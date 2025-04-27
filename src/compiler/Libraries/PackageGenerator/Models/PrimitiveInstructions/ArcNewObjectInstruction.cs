using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcNewObjectInstruction(ArcTypeBase dataType) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x42];

        public ArcTypeBase DataType { get; set; } = dataType;

        public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = [.. Opcode, .. BitConverter.GetBytes(DataType.TypeId)],
            };
        }
    }
}
