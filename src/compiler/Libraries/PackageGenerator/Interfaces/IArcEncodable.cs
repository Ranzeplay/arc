using Arc.Compiler.PackageGenerator.Models;

namespace Arc.Compiler.PackageGenerator.Interfaces
{
    internal interface IArcEncodable
    {
        public byte[] OpCode { get; }

        public ArcEncodeResult Encode();
    }
}
