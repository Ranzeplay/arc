using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeGroupFunctionNode(ArcFunctionDescriptor groupFunction) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupFunction;

        public ArcFunctionDescriptor Descriptor { get; set; } = groupFunction;

        public ArcGroupFunction SyntaxTree { get; set; }

        public ArcPartialGenerationResult GenerationResult { get; set; }

        public override string GetSignature() => "+" + SyntaxTree.Declarator.GetSignature();

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [Descriptor];
    }
}
