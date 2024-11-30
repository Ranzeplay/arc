using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcCompilationUnitStructure
    {
        public IEnumerable<ArcSymbolBase> Symbols { get; set; } = [];
    }
}
