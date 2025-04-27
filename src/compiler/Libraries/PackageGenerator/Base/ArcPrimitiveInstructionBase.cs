using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Base
{
    internal abstract class ArcPrimitiveInstructionBase
    {
        public abstract byte[] Opcode { get; }

        public virtual ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            return new ArcPartialGenerationResult
            {
                GeneratedData = [.. Opcode],
            };
        }
    }
}
