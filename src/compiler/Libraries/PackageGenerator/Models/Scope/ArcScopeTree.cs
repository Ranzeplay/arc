using Arc.Compiler.PackageGenerator.Models.Builtin;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTree(ArcScopeTreeNodeBase? root = null)
    {
        public ArcScopeTreeNodeBase Root { get; set; } = root ?? new ArcRootScopeNode();

        public ICollection<ArcScopeTreeNodeBase> FlattenedNodes => ArcScopeTreeHelpers.FlattenNodes(Root);

        public void MergeRoot(ArcScopeTree other) => ArcScopeTreeHelpers.MergeNodes(Root, other.Root);

        public ICollection<T> GetNodes<T>(Func<T, bool> predicate) where T : ArcScopeTreeNodeBase
        {
            return FlattenedNodes.Where(n => n is T && predicate((T)n)).Cast<T>().ToList();
        }

        public ICollection<T> GetNodes<T>() where T : ArcScopeTreeNodeBase
        {
            return GetNodes<T>(_ => true);
        }
    }
}
