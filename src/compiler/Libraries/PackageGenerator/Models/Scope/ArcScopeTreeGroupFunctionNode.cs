using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeGroupFunctionNode(ArcFunctionDescriptor desc) : ArcScopeTreeFunctionNodeBase(desc)
    {
        public override long Id { get => Descriptor.Id; set => Descriptor.Id = value; }

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupFunction;

        public ArcGroupFunction? SyntaxTree { get; set; }

        public override string SignatureAddend => SyntaxTree!.Declarator.GetSignature();

        public override string Name => SyntaxTree.Declarator.Identifier.Name;

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [Descriptor];
    }
}
