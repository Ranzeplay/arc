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

        public ArcScopeTreeNamespaceNode GetNamespace(IEnumerable<string> names)
        {
            var current = Root;
            foreach (var name in names)
            {
                var ns = current.GetSpecificChild<ArcScopeTreeNamespaceNode>(n => n.Name == name);
                if (ns == null)
                {
                    ns = new ArcScopeTreeNamespaceNode(name);
                    current.AddChild(ns);
                }
                current = ns;
            }
            return (ArcScopeTreeNamespaceNode)current;
        }
    }
}
