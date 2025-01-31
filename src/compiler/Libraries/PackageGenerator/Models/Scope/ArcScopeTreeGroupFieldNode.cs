using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeGroupFieldNode(ArcGroupFieldDescriptor groupField) : ArcScopeTreeNodeBase
    {
        public override long Id { get => GroupField.Id; set => GroupField.Id = value; }

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupField;

        public ArcGroupFieldDescriptor GroupField { get; set; } = groupField;

        public override string SignatureAddend => "+" + GroupField.GetSignature();

        public override string Name => GroupField.Name;

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [GroupField];
    }
}
