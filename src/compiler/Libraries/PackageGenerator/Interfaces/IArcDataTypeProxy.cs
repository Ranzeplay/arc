using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Interfaces
{
    public interface IArcDataTypeProxy
    {
        public string TypeSignature { get; }

        public string ShortName { get; }

        public ulong ProxyTypeId { get; }

        public ulong ActualTypeId { get; }

        public ArcTypeBase ResolvedType { get; }
    }
}
