using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcReturnFromFunctionInstruction(bool withReturnValue) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x35];

        public bool WithReturnValue { get; } = withReturnValue;

        public new ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new()
            {
                GeneratedData = Opcode.Concat([(byte)(WithReturnValue ? 0x01 : 0x00)]),
            };
        }
    }
}
