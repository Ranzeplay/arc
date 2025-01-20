using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeIndividualFunctionNode(ArcFunctionDescriptor desc) : ArcScopeTreeFunctionNodeBase(desc)
    {
        public override long Id { get => Descriptor.Id; init => Descriptor.Id = value; }

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.IndividualFunction;

        public ArcBlockIndependentFunction SyntaxTree { get; set; }

        public override string SignatureAddend => SyntaxTree.Declarator.GetSignature();
    }
}
