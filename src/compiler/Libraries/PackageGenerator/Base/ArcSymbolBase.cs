using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Base;

public abstract class ArcSymbolBase : IArcByteEncodable
{
    public abstract IEnumerable<byte> Encode();
}