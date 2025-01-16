using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcCompilationUnitStructure
    {
        public IEnumerable<ArcSymbolBase> Symbols { get; set; } = [];

        public ArcScopeTree ScopeTree { get; set; } = new();
    }
}
