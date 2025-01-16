using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeDataTypeNode(ArcTypeBase dataType) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.DataType;

        public ArcTypeBase DataType { get; set; } = dataType;
    }
}
