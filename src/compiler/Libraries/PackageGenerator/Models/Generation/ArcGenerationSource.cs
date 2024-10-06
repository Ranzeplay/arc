using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    internal class ArcGenerationSource
    {
        public WeakReference<ArcGenerationContext> ContextReference { get; }

        public IEnumerable<ArcDataSlot> LocalDataSlots { get; } = [];

        public IEnumerable<ArcSymbolBase> AccessibleSymbols { get; set; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public void Merge(ArcGenerationSource generationSource)
        {
            throw new NotImplementedException();
        }

        public void Merge(ArcPartialGenerationResult generationResult)
        {
            throw new NotImplementedException();
        }
    }
}
