namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope
{
    public class ArcRootScopeTreeNode : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Root;

        public override ArcScopeTreeNodeBase Parent { get => null!; set => base.Parent = value; }

        public override string SignatureAddend => "Root";

        public override string Name => SignatureAddend;
    }
}
