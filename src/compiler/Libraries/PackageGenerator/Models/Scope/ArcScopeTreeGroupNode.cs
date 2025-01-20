using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeGroupNode(ArcGroupDescriptor group) : ArcScopeTreeNodeBase
    {
        public override long Id { get => Descriptor.Id; init => Descriptor.Id = value; }

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Group;

        public ArcGroupDescriptor Descriptor { get; set; } = group;

        public ArcGroup SyntaxTree { get; set; }

        public override string SignatureAddend => SyntaxTree.GetSignature();

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [Descriptor];
    }
}
