using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeGroupFunctionNode(ArcFunctionDescriptor desc) : ArcScopeTreeFunctionNodeBase(desc)
    {
        public override long Id { get => Descriptor.Id; init => Descriptor.Id = value; }

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupFunction;

        public ArcGroupFunction SyntaxTree { get; set; }

        public override string SignatureAddend => SyntaxTree.Declarator.GetSignature();
    }
}
