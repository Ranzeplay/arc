using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Interfaces
{
    internal interface IArcPrimitiveInstruction
    {
        public byte[] Opcode { get; }

        public ArcGenerationResult Encode<T>(ArcGenerationSource<T> source)
        {
            return new ArcGenerationResult
            {
                GeneratedData = Opcode,
            };
        }
    }
}
