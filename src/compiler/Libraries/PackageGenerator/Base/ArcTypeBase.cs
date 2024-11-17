namespace Arc.Compiler.PackageGenerator.Base
{
    public class ArcTypeBase : ArcSymbolBase
    {
        public long TypeId { get => Id; }

        public string FullName { get => Name; set => Name = value; }
    }
}
