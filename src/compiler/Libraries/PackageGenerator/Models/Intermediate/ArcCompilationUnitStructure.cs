using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcCompilationUnitStructure(ArcScopeTree scopeTree)
    {
        public ArcScopeTree ScopeTree { get; set; } = scopeTree;

        public List<ArcSymbolBase> Symbols => ScopeTree.FlattenedNodes.SelectMany(n => n.GetSymbols()).ToList();
    }
}
