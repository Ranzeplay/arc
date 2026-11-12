namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public class ArcScopeTreeAnnotationNode : ArcScopeTreeNodeBase
{
    public override string Name => TargetGroup.ShortName;

    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Annotation;

    public required ArcScopeTreeGroupNode TargetGroup { get; set; }

    public override string SignatureAddend => "A" + TargetGroup.ShortName;
}
