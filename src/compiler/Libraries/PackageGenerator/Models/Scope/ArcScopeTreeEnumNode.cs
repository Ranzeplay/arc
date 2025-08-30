using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Models.Scope;

public class ArcScopeTreeEnumNode : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode
{
    public override string Name => SyntaxTree.Name.Name;
    
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Enum;

    public override string SignatureAddend => "E" + Name;
    
    public required ArcBlockEnum SyntaxTree { get; set; }
    
    public Dictionary<ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];
    
    public IEnumerable<ArcScopeTreeEnumMemberNode> Members => Children.OfType<ArcScopeTreeEnumMemberNode>();
    
    public virtual IEnumerable<byte> Encode(ArcScopeTree tree) =>
    [
        (byte)ArcSymbolType.Enum,

        ..BitConverter.GetBytes(Annotations.LongCount()),
        ..Annotations.SelectMany(a => BitConverter.GetBytes(a.Key.Id)),

        ..BitConverter.GetBytes(Members.LongCount()),
        ..Members.SelectMany(m => BitConverter.GetBytes(m.Id))
    ];
}
