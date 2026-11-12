using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public class ArcScopeTreeGroupFieldNode : ArcScopeTreeNodeBase
{
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupField;

    public required ArcDataDeclarationDescriptor DataType { get; set; }

    public Dictionary<Models.Scope.ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];

    public required ArcAccessibility Accessibility { get; set; }

    public required string IdentifierName { get; set; }

    public override string SignatureAddend => "D" + IdentifierName;

    public override string Name => IdentifierName;
}
