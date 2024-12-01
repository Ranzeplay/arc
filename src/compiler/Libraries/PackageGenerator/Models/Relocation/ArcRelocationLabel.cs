using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Relocation
{
    public class ArcRelocationLabel : ArcSymbolBase
    {
        public ArcRelocationLabelType Type { get; set; }

        public long Location { get; set; }
    }
}
