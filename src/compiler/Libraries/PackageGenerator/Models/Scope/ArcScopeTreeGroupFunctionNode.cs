using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeGroupFunctionNode : ArcScopeTreeFunctionNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupFunction;

        public ArcGroupFunction? SyntaxTree { get; set; }

        public override string SignatureAddend => SyntaxTree!.Declarator.GetSignature();

        public override string Name => SyntaxTree.Declarator.Identifier.Name;
    }
}
