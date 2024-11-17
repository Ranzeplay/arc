namespace Arc.Compiler.PackageGenerator.Base
{
    public abstract class ArcSymbolBase
    {
        public long Id { get; internal set; } = new Random().NextInt64();

        public required string Name { get; set; }
    }
}
