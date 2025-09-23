using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate;

public class ArcGroupDerivationLink
{
    public IArcDataTypeProxy Target { get; set; }

    public Dictionary<ArcScopeTreeGenericTypeNode, DimensionProxyType> GenericTypeMap { get; set; }
    
    public record DimensionProxyType
    {
        public IArcDataTypeProxy ProxyType { get; set; }
        public int Dimension { get; set; }
    }
}