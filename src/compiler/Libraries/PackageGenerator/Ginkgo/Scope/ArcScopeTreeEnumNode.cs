using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public class ArcScopeTreeEnumNode : ArcScopeTreeNodeBase
{
    public override string Name => SyntaxTree.Name.Name;
    
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Enum;

    public override string SignatureAddend => "E" + Name;
    
    public required ArcBlockEnum SyntaxTree { get; set; }
    
    public Dictionary<Models.Scope.ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];
    
    public IEnumerable<ArcScopeTreeEnumMemberNode> Members => Children.OfType<ArcScopeTreeEnumMemberNode>();
}
