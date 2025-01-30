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

    public static ArcScopeTree MergeTree(ArcScopeTree tree1, ArcScopeTree tree2, bool overwrite = false)
    {
        var result = new ArcScopeTree();

        var node1 = tree1.Root;
        var node2 = tree2.Root;

        var tree1Namespaces = node1.GetChildren<ArcScopeTreeNamespaceNode>().ToList();
        var tree2Namespaces = node2.GetChildren<ArcScopeTreeNamespaceNode>().ToList();
        foreach (var ns1 in tree1Namespaces)
        {
            var ns2 = tree2Namespaces.FirstOrDefault(n => n.Name == ns1.Name);
            if (ns2 != null)
            {
                var ns2Id = ns2.Id;

                var resultNamespace = MergeNamespaceNodes(ns1, ns2, overwrite);
                result.Root.AddChild(resultNamespace);

                tree2Namespaces.RemoveAll(n => n.Id == ns2Id);
            }
            else
            {
                result.Root.AddChild(ns1);
            }
        }

        foreach (var ns2 in tree2Namespaces)
        {
            result.Root.AddChild(ns2);
        }

        return result;
    }

    // We assume that the namespaces have the same name
    public static ArcScopeTreeNamespaceNode MergeNamespaceNodes(ArcScopeTreeNamespaceNode ns1, ArcScopeTreeNamespaceNode ns2, bool overwrite = false)
    {
        var resultNamespace = new ArcScopeTreeNamespaceNode(ns1.Name);

        var ns1SubNamespaces = ns1.GetChildren<ArcScopeTreeNamespaceNode>().ToList();
        var ns2SubNamespaces = ns2.GetChildren<ArcScopeTreeNamespaceNode>().ToList();
        foreach (var subNs1 in ns1SubNamespaces)
        {
            var subNs2 = ns2SubNamespaces.FirstOrDefault(n => n.Name == subNs1.Name);
            if (subNs2 != null)
            {
                var subNs2Id = subNs2.Id;
                var resultSubNamespace = MergeNamespaceNodes(subNs1, subNs2, overwrite);
                resultNamespace.AddChild(resultSubNamespace);
                ns2SubNamespaces.RemoveAll(n => n.Id == subNs2Id);
            }
            else
            {
                resultNamespace.AddChild(subNs1);
            }
        }

        foreach (var subNs2 in ns2SubNamespaces)
        {
            resultNamespace.AddChild(subNs2);
        }

        var ns1NonNamespaceNodes = ns1.Children.TakeWhile(n => n is not ArcScopeTreeNamespaceNode).ToList();
        var ns2NonNamespaceNodes = ns2.Children.TakeWhile(n => n is not ArcScopeTreeNamespaceNode).ToList();
        foreach (var node1 in ns1NonNamespaceNodes)
        {
            var node2 = ns2NonNamespaceNodes.FirstOrDefault(n => n.Id == node1.Id);
            if (node2 != null)
            {
                if (overwrite)
                {
                    resultNamespace.AddChild(node2);
                    ns2NonNamespaceNodes.Remove(node2);
                }
                else
                {
                    resultNamespace.AddChild(node1);
                }
            }
            else
            {
                resultNamespace.AddChild(node1);
            }
        }

        foreach (var node2 in ns2NonNamespaceNodes)
        {
            resultNamespace.AddChild(node2);
        }

        return resultNamespace;
    }
}
