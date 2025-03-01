using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    class ArcEmptyEncoder : IArcConstantEncoder
    {
        public IEnumerable<byte> Encode(object o) => [];
    }
}
