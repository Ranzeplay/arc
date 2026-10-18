namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public class ArcScopeTree(ArcScopeTreeNodeBase? root = null)
{
    public ArcScopeTreeNodeBase Root { get; set; } = root ?? new ArcRootScopeTreeNode();

    public ICollection<ArcScopeTreeNodeBase> FlattenedNodes => Root.FlattenNodes().DistinctBy(n => n.Id).ToList();

    public void MergeRoot(ArcScopeTree other, bool overwrite = false) => Root = ArcGinkgoScopeTreeHelpers.MergeTree(this, other, overwrite).Root;

    public ICollection<T> GetNodes<T>(Func<T, bool> predicate) where T : ArcScopeTreeNodeBase
    {
        return [.. FlattenedNodes.Where(n => n is T t && predicate(t)).Cast<T>()];
    }

    public ICollection<T> GetNodes<T>() where T : ArcScopeTreeNodeBase
    {
        return GetNodes<T>(_ => true);
    }

    public ArcScopeTreeNamespaceNode? GetNamespace(IEnumerable<string> names)
    {
        var current = Root;
        foreach (var name in names)
        {
            var ns = current.GetSpecificChild<ArcScopeTreeNamespaceNode>(n => n.Name == name);
            if (ns == null)
            {
                return null;
            }
            current = ns;
        }
        return current as ArcScopeTreeNamespaceNode;
    }

    public ArcScopeTreeNamespaceNode? GetNamespaceByName(string name)
    {
        return GetNamespace(name.Split("::"));
    }

    public T? GetNode<T>(IEnumerable<string> names) where T : ArcScopeTreeNodeBase
        => GetNodes<T>(names).FirstOrDefault();

    public IEnumerable<T> GetNodes<T>(IEnumerable<string> names) where T : ArcScopeTreeNodeBase
    {
        var current = Root;
        var path = names.ToList();
        if (path.Count == 0) return [];
        foreach (var name in path.Take(path.Count - 1))
        {
            var node = current.GetSpecificChild(n => n.Name == name &&
                n is ArcScopeTreeNamespaceNode or ArcScopeTreeGroupNode);
            if (node == null)
            {
                return [];
            }
            current = node;
        }

        return current.GetSpecificChildren<T>(n => n.Name == path[^1]);
    }

    public T? GetNodeByName<T>(string name) where T : ArcScopeTreeNodeBase => Root.GetSpecificChild<T>(n => n.Name == name, true);

    public ArcScopeTreeNodeBase? GetNodeByName(string name) => GetNodeByName<ArcScopeTreeNodeBase>(name);
        
    public ArcScopeTreeNodeBase? GetNodeById(ulong id) => FlattenedNodes.FirstOrDefault(n => n.Id == id);
}
