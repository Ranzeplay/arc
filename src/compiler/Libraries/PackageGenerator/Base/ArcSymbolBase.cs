using Arc.Compiler.PackageGenerator.Models;

namespace Arc.Compiler.PackageGenerator.Base
{
    public abstract class ArcSymbolBase
    {
        public ulong Id { get; internal set; } = (ulong)new Random().NextInt64();

        public required string Name { get; set; }

        public ArcSymbolScope Scope { get; set; } = new() { Type = ArcSymbolScopeType.CurrentPackage };
    }
}
