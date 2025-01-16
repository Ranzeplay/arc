using Arc.Compiler.PackageGenerator.Models.Builtin;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTree(ArcScopeTreeNodeBase? root = null)
    {
        public ArcScopeTreeNodeBase Root { get; set; } = root ?? new ArcRootScopeNode();

        public ICollection<ArcScopeTreeNodeBase> FlattenNodes() => ArcScopeTreeHelpers.FlattenNodes(Root);

        public void MergeRoot(ArcScopeTree other) => ArcScopeTreeHelpers.MergeNode(Root, other.Root);
    }
}
