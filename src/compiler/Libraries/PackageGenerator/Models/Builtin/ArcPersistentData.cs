namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal static class ArcPersistentData
    {
        public static IEnumerable<ArcBaseType> BaseTypes = [
            new ArcBaseType { Id = 0, Name = "none" },
            new ArcBaseType { Id = 1, Name = "any" },
            new ArcBaseType { Id = 2, Name = "int" },
            new ArcBaseType { Id = 3, Name = "decimal" },
            new ArcBaseType { Id = 4, Name = "char" },
            new ArcBaseType { Id = 5, Name = "string" },
            new ArcBaseType { Id = 6, Name = "bool" },
        ];
    }
}
