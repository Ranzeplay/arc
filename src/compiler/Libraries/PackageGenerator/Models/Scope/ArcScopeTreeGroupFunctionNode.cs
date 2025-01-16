using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeGroupFunctionNode(ArcFunctionDescriptor groupFunction) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupFunction;

        public ArcFunctionDescriptor GroupFunction { get; set; } = groupFunction;
    }
}
