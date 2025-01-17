namespace Arc.Compiler.PackageGenerator.Base
{
    public class ArcTypeBase(string identifier) : ArcSymbolBase
    {
        public long TypeId { get => Id; }

        public string Identifier { get; set; } = identifier;

        public string FullName { get => Name; set => Name = value; }
    }
}
