using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Scope;

internal static class ArcScopeTreeHelpers
{
    public static ICollection<ArcScopeTreeNodeBase> FlattenNodes(ArcScopeTreeNodeBase node)
    {
        var nodes = new List<ArcScopeTreeNodeBase>
        {
            node
        };

        foreach (var child in node.Children)
        {
            nodes.AddRange(FlattenNodes(child));
        }

        return nodes;
    }

    public static ArcScopeTreeNodeBase MergeNodes(ArcScopeTreeNodeBase node1, ArcScopeTreeNodeBase node2)
    {
        MergeNamespace(node1.GetSpecificChild<ArcScopeTreeNamespaceNode>(_ => true)!, node2.GetSpecificChild<ArcScopeTreeNamespaceNode>(_ => true)!);
        return node1;
    }

    public static ArcScopeTreeNamespaceNode? MergeNamespace(ArcScopeTreeNamespaceNode ns1, ArcScopeTreeNamespaceNode ns2)
    {
        if (ns1.Name == ns2.Name)
        {
            foreach(var ns in ns1.GetChildren<ArcScopeTreeNamespaceNode>())
            {
                var ns2ChildNamespace = ns2.GetChildren<ArcScopeTreeNamespaceNode>().FirstOrDefault(n => n.Name == ns.Name);
                if (ns2ChildNamespace != null)
                {
                    MergeNamespace(ns, ns2ChildNamespace);
                    ns2.Children.Remove(ns2ChildNamespace);
                }
            }

            foreach (var ns in ns2.Children)
            {
                ns1.AddChild(ns);
            }
        }

        return ns1;
    }
}
