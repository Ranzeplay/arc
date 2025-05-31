using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeIndividualFunctionNode : ArcScopeTreeFunctionNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.IndividualFunction;

        public ArcBlockIndependentFunction SyntaxTree { get; set; }

        public override string SignatureAddend => SyntaxTree.Declarator.GetSignature();

        public override string Name => SyntaxTree.Declarator.Identifier.Name;
    }
}
