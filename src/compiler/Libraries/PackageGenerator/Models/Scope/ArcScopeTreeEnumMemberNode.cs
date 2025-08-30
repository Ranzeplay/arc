using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Models.Scope;

public class ArcScopeTreeEnumMemberNode : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode
{
    public override string Name => SyntaxTree.Name.Name;
    
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.EnumMember;
    
    public override string SignatureAddend => "K" + Name;
    
    public Dictionary<ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];
    
    public required ArcEnumMember SyntaxTree { get; set; }


    public IEnumerable<byte> Encode(ArcScopeTree tree) =>
    [
        (byte)ArcSymbolType.EnumMember,

        ..BitConverter.GetBytes(Annotations.LongCount()),
        ..Annotations.SelectMany(a => BitConverter.GetBytes(a.Key.Id)),
    ];
}
