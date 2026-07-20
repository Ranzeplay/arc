using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

public class ArcGinkgoUnitGenerationContext
{
    public ArcScopeTree ScopeTree { get; set; }
    
    public IEnumerable<ArcScopeTreeNamespaceNode> LinkedNamespaces { get; set; }
    
    public ArcCompilationUnit CompilationUnit { get; set; }
    
    public IEnumerable<ArcScopeTreeNodeBase> LinkedNodes
    {
        get
        {
            return LinkedNamespaces
                .SelectMany(lns => lns.Children);
        }
    }
    
    public IEnumerable<IArcDataTypeProxy> LinkedTypes { get; }
    
    public IEnumerable<IArcDataTypeProxy> LinkedGenericTypes { get; }
    
    public string UnitName => CompilationUnit.Name;
}