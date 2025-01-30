using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal static class ArcPersistentData
    {
        public static IEnumerable<ArcBaseType> BaseTypes = [
            new ArcBaseType(0, "none"){Name = "none"},
            new ArcBaseType(1, "any"){Name = "any"},
            new ArcBaseType(2, "int") {Name = "int" },
            new ArcBaseType(3, "decimal") {Name = "decimal" },
            new ArcBaseType(4, "char") {Name = "char"},
            new ArcBaseType(5, "string") {Name = "string"},
            new ArcBaseType(6, "bool") {Name = "bool"},
        ];

        public static ArcScopeTree BaseTypeScopeTree
        {
            get
            {
                var tree = new ArcScopeTree();
                var node = tree.Root.AddChild(new ArcScopeTreeNamespaceNode("Arc"));
                node = node.AddChild(new ArcScopeTreeNamespaceNode("Base"));
                node.AddChildren(BaseTypes.Select(t =>
                {
                    var node = new ArcScopeTreeDataTypeNode(t);
                    node.Id = t.Id;
                    return node;
                }));
                return tree;
            }
        }
    }
}
