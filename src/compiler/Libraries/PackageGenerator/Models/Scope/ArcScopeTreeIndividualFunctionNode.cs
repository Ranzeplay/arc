using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeIndividualFunctionNode(ArcFunctionDescriptor function) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.IndividualFunction;

        public ArcFunctionDescriptor Function { get; set; } = function;
    }
}
