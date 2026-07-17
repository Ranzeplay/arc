using Arc.Compiler.PackageGenerator.Ginkgo.Interface;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public class ArcScopeTreeNamespaceNode(string name) : ArcScopeTreeNodeBase, IArcEncodableGinkgoScopeTreeNode
{
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Namespace;

    public override string SignatureAddend => "N" + Name;

    public override string Name => name;

    public IEnumerable<byte> Encode(ArcScopeTree tree) =>
    [
        (byte)ArcSymbolType.Namespace,
    ];

    public ArcScopeTree GetIsolatedTree()
    {
        var tree = new ArcScopeTree();
        var current = tree.Root;
        var ancestors = GetAncestors().Reverse();
        foreach (var ancestor in ancestors)
        {
            current = current.AddChild(ancestor);
        }
        return tree;
    }
}