using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Interfaces
{
    public interface IArcEncodableScopeTreeNode
    {
        public IEnumerable<byte> Encode(ArcScopeTree tree);
    }
}
