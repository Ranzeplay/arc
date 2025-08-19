using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcCompilationUnitStructure(ArcScopeTree scopeTree, ArcCompilationUnit unit, ArcScopeTree globalScopeTree)
    {
        public ArcCompilationUnit CompilationUnit { get; set; } = unit;

        public ArcScopeTree ScopeTree { get; set; } = scopeTree;
        
        public ArcScopeTree GlobalScopeTree { get; set; } = globalScopeTree;

        public IEnumerable<ArcScopeTreeNamespaceNode> LinkedNamespaces =>
            CompilationUnit.LinkedSymbols.Select(ls => GlobalScopeTree.GetNamespace(ls.Identifier.Namespace))
                .Concat([GlobalScopeTree.GetNamespace(["Arc", "Base"]), CurrentNamespace])
                .Where(n => n != null)
                .Select(n => n!)
                .Distinct();
        
        public IEnumerable<ArcScopeTreeNodeBase> DirectlyAccessibleNodes => [
            ..ScopeTree.GetNamespace(CompilationUnit.Namespace.Identifier.Namespace).Children,
            ..LinkedNamespaces.SelectMany(lns => lns.Children)
            ];

        public ArcScopeTreeNamespaceNode CurrentNamespace => ScopeTree.GetNamespace(CompilationUnit.Namespace.Identifier.Namespace);
    }
}
