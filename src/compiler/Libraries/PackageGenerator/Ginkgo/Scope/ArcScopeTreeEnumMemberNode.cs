using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public class ArcScopeTreeEnumMemberNode : ArcScopeTreeNodeBase
{
    public override string Name => SyntaxTree.Name.Name;
    
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.EnumMember;
    
    public override string SignatureAddend => "K" + Name;
    
    public Dictionary<Models.Scope.ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];
    
    public required ArcEnumMember SyntaxTree { get; set; }
}
