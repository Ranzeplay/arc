using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcNewObjectInstruction(ArcScopeTreeDataTypeNode dataType) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x42];

        public ArcScopeTreeDataTypeNode DataType { get; set; } = dataType;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = [.. Opcode, .. BitConverter.GetBytes(DataType.ComplexTypeGroup!.Id)],
            };
        }
    }
}
