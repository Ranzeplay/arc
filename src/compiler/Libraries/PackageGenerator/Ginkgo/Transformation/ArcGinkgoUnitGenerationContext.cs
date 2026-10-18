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
    
    public IEnumerable<IArcDataTypeProxy> LinkedTypes => LinkedNodes.OfType<IArcDataTypeProxy>();
    
    public IEnumerable<IArcDataTypeProxy> LinkedGenericTypes { get; set; } = [];

    // Pool indices are assigned once per unit, shared by all generated functions.
    public List<(ulong TypeId, object Value)> Constants { get; } = [];

    public uint InternConstant(ulong typeId, object value)
    {
        var index = Constants.FindIndex(c => c.TypeId == typeId && Equals(c.Value, value));
        if (index >= 0) return checked((uint)index);
        Constants.Add((typeId, value));
        return checked((uint)(Constants.Count - 1));
    }
    
    public string UnitName => CompilationUnit.Name;
}
