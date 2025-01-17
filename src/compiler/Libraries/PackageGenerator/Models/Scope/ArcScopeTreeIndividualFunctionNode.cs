using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeIndividualFunctionNode(ArcFunctionDescriptor function) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.IndividualFunction;

        public ArcFunctionDescriptor Descriptor { get; set; } = function;

        public ArcBlockIndependentFunction SyntaxTree { get; set; }

        public ArcPartialGenerationResult GenerationResult { get; set; }

        public override string GetSignature() => "+" + SyntaxTree.Declarator.GetSignature();

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [Descriptor];
    }
}
