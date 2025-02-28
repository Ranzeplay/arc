using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcFunctionCallInstruction(ulong functionSymbolId, uint parameterCount) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x36];

        public ulong FunctionSymbolId { get; set; } = functionSymbolId;

        public uint ParameterCount { get; set; } = parameterCount;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = [.. Opcode, .. BitConverter.GetBytes(FunctionSymbolId), .. BitConverter.GetBytes(ParameterCount)],
            };
        }
    }
}
