using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Interface;

public interface IArcGinkgoGenerationContext<out T> where T : ArcScopeTreeNodeBase
{
    public ArcGinkgoUnitGenerationContext GlobalContext { get; }
    
    public T CurrentNode { get; }
    
    public ArcScopeTree GlobalScopeTree => GlobalContext.ScopeTree;
    
    public IEnumerable<IArcDataTypeProxy> LinkedTypes { get; }
    
    public IEnumerable<IArcDataTypeProxy> LinkedGenericTypes { get; }

    public IEnumerable<ArcScopeTreeNodeBase> DirectlyAccessibleNodes => GlobalContext.LinkedNodes;
}