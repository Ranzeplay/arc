using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeNamespaceNode(string name) : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Namespace;

        public override string SignatureAddend => "N" + Name;

        public override string Name => name;

        public IEnumerable<byte> Encode(ArcScopeTree tree)
        {
            var iterResult = new List<byte>();
            iterResult.Add((byte)ArcSymbolType.Namespace);
            iterResult.AddRange(new ArcStringEncoder().Encode(Signature));

            return iterResult;
        }

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
}
