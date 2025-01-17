using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcCompilationUnitStructure
    {
        public List<ArcSymbolBase> Symbols { get; set; } = [];

        public ArcScopeTree ScopeTree { get; set; } = new();

        public void OverwriteSymbolsUsingScopeTree()
        {
            Symbols = [];

            foreach (var node in ScopeTree.FlattenedNodes)
            {
                Symbols.AddRange(node.GetSymbols());
            }
        }
    }
}
