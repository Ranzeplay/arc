using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    internal interface IArcSymbolContainer
    {
        public Dictionary<long, ArcSymbolBase> Symbols { get; }
    }
}
