using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Symbol;

public class ArcSymbol : IArcByteEncodable
{
    public ArcSymbolType SymbolType  { get; set; }

    public required ArcSymbolBase Symbol { get; set; }
    
    public IEnumerable<byte> Encode()
    {
        return [(byte) SymbolType, ..Symbol.Encode()];
    }
}