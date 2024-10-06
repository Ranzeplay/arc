using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal interface IArcPrimitiveInstruction
    {
        public abstract byte[] Opcode { get; }

        public ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = Opcode,
            };
        }
    }
}
