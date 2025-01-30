using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeNamespaceNode(string name) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Namespace;

        public string Name { get; set; } = name;

        public override string SignatureAddend => "N" + Name;

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [new ArcNamespaceDescriptor() { Name = Name }];

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
