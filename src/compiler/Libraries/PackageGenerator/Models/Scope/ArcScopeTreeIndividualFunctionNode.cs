using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeIndividualFunctionNode(ArcFunctionDescriptor desc) : ArcScopeTreeFunctionNodeBase(desc)
    {
        public override long Id { get => Descriptor.Id; set => Descriptor.Id = value; }

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.IndividualFunction;

        public ArcBlockIndependentFunction SyntaxTree { get; set; }

        public override string SignatureAddend => SyntaxTree.Declarator.GetSignature();

        public override string Name => SyntaxTree.Declarator.Identifier.Name;
    }
}
