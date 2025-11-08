using Arc.Compiler.PackageGenerator.Models;

namespace Arc.Compiler.PackageGenerator.Base
{
    public class ArcTypeBase(string identifier)
    {
        public ulong TypeId { get; set; } = (ulong)new Random().NextInt64();

        public string Identifier { get; set; } = identifier;

        public string FullName { get; set; } = identifier;

        public ArcSymbolScope Scope { get; set; } = new() { Type = ArcSymbolScopeType.CurrentPackage, Remark = "Default scope for types" };

        protected bool Equals(ArcTypeBase other)
        {
            return TypeId == other.TypeId && Scope.Equals(other.Scope);
        }
    }
}
