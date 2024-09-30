using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Interfaces
{
    internal abstract class ArcPrimitiveInstructionBase
    {
        public abstract byte[] Opcode { get; }

        public ArcGenerationResult Encode<T>(ArcGenerationSource<T> source)
        {
            return new ArcGenerationResult
            {
                GeneratedData = Opcode,
            };
        }
    }
}
