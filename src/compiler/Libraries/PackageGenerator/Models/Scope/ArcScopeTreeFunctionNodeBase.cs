using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal abstract class ArcScopeTreeFunctionNodeBase(ArcFunctionDescriptor descriptor) : ArcScopeTreeNodeBase
    {
        public ArcFunctionDescriptor Descriptor { get; set; } = descriptor;

        public ArcPartialGenerationResult GenerationResult { get; set; }

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [Descriptor];
    }
}
