namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal static class PersistentData
    {
        public static IEnumerable<ArcBaseType> BaseTypes = [
            new ArcBaseType { Name = "none" },
            new ArcBaseType { Name = "any" },
            new ArcBaseType { Name = "int" },
            new ArcBaseType { Name = "decimal" },
            new ArcBaseType { Name = "char" },
            new ArcBaseType { Name = "string" },
            new ArcBaseType { Name = "bool" },
        ];
    }
}
