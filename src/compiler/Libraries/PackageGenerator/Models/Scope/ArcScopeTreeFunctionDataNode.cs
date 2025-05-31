using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeFunctionDataNode(ArcDataDeclarator declarator) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.FunctionData;

        public ArcDataDeclarator SyntaxTree { get; set; } = declarator;

        public override string SignatureAddend => "S" + SyntaxTree.GetSignature();

        public override string Name => SyntaxTree.Identifier.Name;
    }
}
