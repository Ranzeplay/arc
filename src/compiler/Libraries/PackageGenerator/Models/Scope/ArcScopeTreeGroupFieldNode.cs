using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeGroupFieldNode(ArcGroupFieldDescriptor groupField) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupField;

        public ArcGroupFieldDescriptor GroupField { get; set; } = groupField;

        public override string GetSignature()
        {
            return "+" + GroupField.GetSignature();
        }

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [GroupField];
    }
}
