using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal class ArcRootScopeNode : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Root;

        public override ArcScopeTreeNodeBase Parent { get => null!; set => base.Parent = value; }
    }
}
