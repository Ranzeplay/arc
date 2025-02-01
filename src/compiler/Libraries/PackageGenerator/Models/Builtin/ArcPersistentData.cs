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

        public static ArcBaseType NoneType => new ArcBaseType(0, "none") { Name = "none" };
        public static ArcBaseType AnyType => new ArcBaseType(1, "any") { Name = "any" };
        public static ArcBaseType IntType => new ArcBaseType(2, "int") { Name = "int" };
        public static ArcBaseType DecimalType => new ArcBaseType(3, "decimal") { Name = "decimal" };
        public static ArcBaseType CharType => new ArcBaseType(4, "char") { Name = "char" };
        public static ArcBaseType StringType => new ArcBaseType(5, "string") { Name = "string" };
        public static ArcBaseType BoolType => new ArcBaseType(6, "bool") { Name = "bool" };

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
