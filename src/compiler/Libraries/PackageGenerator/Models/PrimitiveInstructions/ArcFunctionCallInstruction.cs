using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcFunctionCallInstruction(long functionSymbolId, int parameterCount) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x36];

        public long FunctionSymbolId { get; set; } = functionSymbolId;

        public int ParameterCount { get; set; } = parameterCount;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = [.. Opcode, .. BitConverter.GetBytes(FunctionSymbolId), .. BitConverter.GetBytes(ParameterCount)],
            };
        }
    }
}
