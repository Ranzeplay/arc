using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal static class ArcPersistentData
    {
        public static IEnumerable<ArcBaseType> BaseTypes = [
            NoneType,
            AnyType,
            IntType,
            DecimalType,
            CharType,
            StringType,
            BoolType
        ];

        public static ArcBaseType NoneType => new(0, "none") { Name = "none" };
        public static ArcBaseType AnyType => new(1, "any") { Name = "any" };
        public static ArcBaseType IntType => new(2, "int") { Name = "int" };
        public static ArcBaseType DecimalType => new(3, "decimal") { Name = "decimal" };
        public static ArcBaseType CharType => new(4, "char") { Name = "char" };
        public static ArcBaseType StringType => new(5, "string") { Name = "string" };
        public static ArcBaseType BoolType => new(6, "bool") { Name = "bool" };

        public static ArcScopeTree BaseTypeScopeTree
        {
            get
            {
                var tree = new ArcScopeTree();
                var node = tree.Root.AddChild(new ArcScopeTreeNamespaceNode("Arc"));
                node = node.AddChild(new ArcScopeTreeNamespaceNode("Base"));
                node.AddChildren(BaseTypes.Select(t =>
                {
                    var node = new ArcScopeTreeDataTypeNode(t, t.Name);
                    node.Id = t.Id;
                    return node;
                }));
                return tree;
            }
        }
    }
}
