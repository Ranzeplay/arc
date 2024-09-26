using Arc.Compiler.PackageGenerator.Models;

namespace Arc.Compiler.PackageGenerator.Interfaces
{
    internal interface IArcPrimitiveInstruction
    {
        public byte[] Opcode { get; }

        public ArcGeneratorContext Encode()
        {
            return new ArcGeneratorContext
            {
                GeneratedData = Opcode,
                RelocationDescriptors = [],
                Labels = []
            };
        }
    }
}
