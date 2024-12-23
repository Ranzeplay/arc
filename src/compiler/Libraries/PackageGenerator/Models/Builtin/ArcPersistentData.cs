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
    }
}
