using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeFunctionDataNode(ArcDataDeclarator declarator) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.FunctionData;

        public ArcDataDeclarator SyntaxTree { get; set; } = declarator;

        public override string GetSignature() => "+S" + SyntaxTree.GetSignature();

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [];
    }
}
