using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public class ArcScopeTreeGroupNode : ArcScopeTreeNodeBase
{
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Group;

    public ArcGroup SyntaxTree { get; set; }

    public override string SignatureAddend => ShortName;

    public override string Name => ShortName;

    public string ShortName { get; set; }

    public IEnumerable<ArcScopeTreeGroupFunctionNode> Functions => GetChildren<ArcScopeTreeGroupFunctionNode>();

    public IEnumerable<ArcScopeTreeLifecycleFunctionNode> LifecycleFunctions => GetChildren<ArcScopeTreeLifecycleFunctionNode>();

    public IEnumerable<ArcScopeTreeGroupFieldNode> Fields => GetChildren<ArcScopeTreeGroupFieldNode>();

    public IEnumerable<ArcScopeTreeGroupNode> Groups => GetChildren<ArcScopeTreeGroupNode>();

    public Dictionary<ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];
        
    public IEnumerable<ArcGroupDerivationLink> Derivations { get; set; } = [];

    public ArcAccessibility Accessibility { get; set; } = ArcAccessibility.Private;

    public IEnumerable<ArcScopeTreeGenericTypeNode> GenericTypes => GetChildren<ArcScopeTreeGenericTypeNode>();
}
