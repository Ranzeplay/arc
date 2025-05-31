using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcFunctionCallInstruction(ulong functionSymbolId, uint parameterCount, IEnumerable<ArcDataType> specializedGenericTypes) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x36];

        public ulong FunctionSymbolId { get; set; } = functionSymbolId;

        public uint ParameterCount { get; set; } = parameterCount;

        public IEnumerable<ArcDataType> SpecializedGenericTypes { get; set; } = specializedGenericTypes;

        public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            var specializedGenericTypeId = SpecializedGenericTypes.Select(t => ArcDataTypeHelper.GetDataType(source, t)?.ResolvedType.TypeId ?? 0);

            return new ArcPartialGenerationResult
            {
                GeneratedData = [
                    .. Opcode, 
                    .. BitConverter.GetBytes(FunctionSymbolId), 
                    .. specializedGenericTypeId.SelectMany(BitConverter.GetBytes), 
                    .. BitConverter.GetBytes(ParameterCount)
                ],
            };
        }
    }
}
