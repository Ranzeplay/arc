using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.PackageGenerator.Models.Scope;

public class ArcScopeTreeGroupConstructorNode : ArcScopeTreeFunctionNodeBase
{
    public override string Name { get; } = $"ctor_{Guid.NewGuid()}";
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupConstructor;
    public override string SignatureAddend => "J";

    public ArcSourceCodeParser.Arc_group_constructorContext SyntaxTree { get; set; }
}
