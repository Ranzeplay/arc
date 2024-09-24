using Arc.Compiler.PackageGenerator.Models;

namespace Arc.Compiler.PackageGenerator.Interfaces
{
    internal interface IArcPrimitiveInstruction
    {
        public byte[] Opcode { get; }

        public ArcPartialEncodeResult Encode()
        {
            return new ArcPartialEncodeResult
            {
                Bytes = Opcode,
                RelocationDescriptors = [],
                Labels = []
            };
        }
    }
}
