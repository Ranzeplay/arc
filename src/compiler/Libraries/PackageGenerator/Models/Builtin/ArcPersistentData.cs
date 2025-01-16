using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal static class ArcPersistentData
    {
        public static IEnumerable<ArcBaseType> BaseTypes = [
            new ArcBaseType { Id = 0, Name = "none", Scope = new(){ Type = ArcSymbolScopeType.Language } },
            new ArcBaseType { Id = 1, Name = "any", Scope = new(){ Type = ArcSymbolScopeType.Language } },
            new ArcBaseType { Id = 2, Name = "int", Scope = new(){ Type = ArcSymbolScopeType.Language } },
            new ArcBaseType { Id = 3, Name = "decimal", Scope = new(){ Type = ArcSymbolScopeType.Language } },
            new ArcBaseType { Id = 4, Name = "char", Scope = new(){ Type = ArcSymbolScopeType.Language } },
            new ArcBaseType { Id = 5, Name = "string", Scope = new(){ Type = ArcSymbolScopeType.Language } },
            new ArcBaseType { Id = 6, Name = "bool", Scope = new(){ Type = ArcSymbolScopeType.Language } },
        ];

        public static ArcScopeTree BaseTypeScopeTree
        {
            get
            {
                var tree = new ArcScopeTree();
                var node = tree.Root.AddChild(new ArcScopeTreeNamespaceNode("Arc"));
                node = node.AddChild(new ArcScopeTreeNamespaceNode("Base"));
                node.AddChildren(BaseTypes.Select(t => new ArcScopeTreeDataTypeNode(t)));
                return tree;
            }
        }
    }
}
