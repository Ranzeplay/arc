using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcNewObjectInstruction(ArcTypeBase dataType, IEnumerable<ArcDataType> specializedGenericTypes) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x42];

        public ArcTypeBase DataType { get; set; } = dataType;

        public IEnumerable<ArcDataType> SpecializedGenericTypes { get; set; } = specializedGenericTypes;

        public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            var specializedGenericTypeId = SpecializedGenericTypes.Select(t => ArcDataTypeHelper.GetDataType(source, t)?.ResolvedType.TypeId ?? 0);

            return new ArcPartialGenerationResult
            {
                GeneratedData = [
                    .. Opcode,
                    .. BitConverter.GetBytes(DataType.TypeId),
                    .. specializedGenericTypeId.SelectMany(BitConverter.GetBytes)
                ],
            };
        }
    }
}
