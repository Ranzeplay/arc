using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcCompilationUnitStructure(ArcScopeTree scopeTree, ArcCompilationUnit unit)
    {
        public ArcCompilationUnit CompilationUnit { get; set; } = unit;

        public ArcScopeTree ScopeTree { get; set; } = scopeTree;

        public List<ArcSymbolBase> Symbols => ScopeTree.FlattenedNodes.SelectMany(n => n.GetSymbols()).ToList();
    }
}
