namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal static class PersistentData
    {
        public static IEnumerable<ArcBaseType> BaseTypes = [
            new ArcBaseType { TypeId = 0, FullName = "none" },
            new ArcBaseType { TypeId = 1, FullName = "any" },
            new ArcBaseType { TypeId = 2, FullName = "int" },
            new ArcBaseType { TypeId = 3, FullName = "decimal" },
            new ArcBaseType { TypeId = 4, FullName = "char" },
            new ArcBaseType { TypeId = 5, FullName = "string" },
            new ArcBaseType { TypeId = 6, FullName = "bool" },
        ];
    }
}
