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

    public static void MergeNode(ArcScopeTreeNodeBase node, ArcScopeTreeNodeBase otherNode)
    {
        var otherNodePosition = otherNode;
        var currentNodePosition = node;

        foreach (var otherChild in otherNode.Children)
        {
            var found = false;
            foreach (var currentChild in currentNodePosition.Children)
            {
                if (currentChild.Equals(otherChild))
                {
                    MergeNode(currentChild, otherChild);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                currentNodePosition.AddChild(otherChild);
            }
        }
    }
}