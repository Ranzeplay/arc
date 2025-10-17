using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal static class ArcPersistentData
    {
        private static readonly IEnumerable<ArcBaseType> BaseTypes = [
            NoneType,
            AnyType,
            IntType,
            DecimalType,
            CharType,
            StringType,
            BoolType,
            ByteType,
            FunctionType
        ];

        public static ArcBaseType NoneType => new(0, "none");
        public static ArcBaseType AnyType => new(1, "any");
        public static ArcBaseType IntType => new(2, "int");
        public static ArcBaseType DecimalType => new(3, "decimal");
        public static ArcBaseType CharType => new(4, "char");
        public static ArcBaseType StringType => new(5, "string");
        public static ArcBaseType BoolType => new(6, "bool");
        public static ArcBaseType ByteType => new(7, "byte");
        public static ArcBaseType FunctionType => new(8, "func");


        public static ArcScopeTree BaseTypeScopeTree
        {
            get
            {
                var tree = new ArcScopeTree();
                var node = tree.Root.AddChild(new ArcScopeTreeNamespaceNode("Arc"));
                node = node.AddChild(new ArcScopeTreeNamespaceNode("Base"));
                node.AddChildren(BaseTypes.Select(t =>
                {
                    var node = new ArcScopeTreeDataTypeNode(ArcDataTypeType.Primitive, t, t.Identifier);
                    node.Id = t.TypeId;
                    return node;
                }));
                return tree;
            }
        }
    }
}
