using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope;

public class ArcScopeTreeLifecycleFunctionNode : ArcScopeTreeFunctionNodeBase
{
    public override string Name => SyntaxTree.LifecycleStage.ToString().ToLower();
    
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupLifecycleFunction;

    public override string SignatureAddend => $"L{Name}";
    
    public ArcGroupLifecycleFunction SyntaxTree { get; }
}
