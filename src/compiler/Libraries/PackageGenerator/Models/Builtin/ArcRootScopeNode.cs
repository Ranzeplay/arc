using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    public class ArcRootScopeNode : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Root;

        public override ArcScopeTreeNodeBase Parent { get => null!; set => base.Parent = value; }

        public override string SignatureAddend => "Root";

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [];
    }
}
