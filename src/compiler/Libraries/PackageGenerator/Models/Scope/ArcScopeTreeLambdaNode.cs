using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Scope;

public class ArcScopeTreeLambdaNode : ArcScopeTreeFunctionNodeBase
{
    public override string Name { get; } = $"Lambda${Guid.NewGuid().ToString("N")[..8]}";
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Lambda;
    public override string SignatureAddend => "Q" + Name;

    public ArcLambdaExpression SyntaxTree { get; set; }
    
    public new ArcAccessibility Accessibility => ArcAccessibility.Private;
    
    public ArcPartialGenerationResult GenerationResult { get; set; }
}