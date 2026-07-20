using Arc.Compiler.PackageGenerator.Ginkgo.Scope;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Interface
{
    public interface IArcEncodableGinkgoScopeTreeNode
    {
        public IEnumerable<byte> Encode(ArcScopeTree tree);
    }
}
