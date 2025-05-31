using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcCompilationUnitStructure(ArcScopeTree scopeTree, ArcCompilationUnit unit)
    {
        public ArcCompilationUnit CompilationUnit { get; set; } = unit;

        public ArcScopeTree ScopeTree { get; set; } = scopeTree;

        public List<ArcScopeTreeNamespaceNode> LinkedNamespaces { get; set; } = [];

        public IEnumerable<ArcScopeTreeNodeBase> DirectlyAccessibleNodes => [
            ..ScopeTree.GetNamespace(CompilationUnit.Namespace.Identifier.Namespace).Children,
            ..LinkedNamespaces.SelectMany(lns => lns.Children)
            ];

        public ArcScopeTreeNamespaceNode GetCurrentNamespace => ScopeTree.GetNamespace(CompilationUnit.Namespace.Identifier.Namespace);
    }
}
