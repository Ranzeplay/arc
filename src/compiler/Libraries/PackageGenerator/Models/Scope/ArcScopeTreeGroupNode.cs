using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeGroupNode(ArcGroupDescriptor group) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Group;

        public ArcGroupDescriptor Descriptor { get; set; } = group;

        public ArcGroup SyntaxTree { get; set; }

        public override string GetSignature() => "+G" + SyntaxTree.Identifier.ToString();

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [Descriptor];
    }
}
