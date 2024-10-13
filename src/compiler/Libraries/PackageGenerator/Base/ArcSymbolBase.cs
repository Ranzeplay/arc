using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Base
{
    internal abstract class ArcSymbolBase : IArcLocatable
    {
        public long Id { get; internal set; } = new Random().NextInt64();

        public required string Name { get; set; }
    }
}
