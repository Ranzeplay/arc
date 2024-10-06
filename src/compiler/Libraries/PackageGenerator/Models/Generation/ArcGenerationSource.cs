using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    internal class ArcGenerationSource
    {
        public WeakReference<ArcGenerationContext> ContextReference { get; }

        public IEnumerable<ArcDataSlot> LocalDataSlots { get; set; } = [];

        public IEnumerable<ArcSymbolBase> AccessibleSymbols { get; set; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public void Merge(ArcGenerationSource generationSource)
        {
            LocalDataSlots = LocalDataSlots.Concat(generationSource.LocalDataSlots);
            AccessibleSymbols = AccessibleSymbols.Concat(generationSource.AccessibleSymbols);
        }

        public void Merge(ArcPartialGenerationResult generationResult)
        {
            LocalDataSlots = LocalDataSlots.Concat(generationResult.DataSlots);
            AccessibleSymbols = AccessibleSymbols.Concat(generationResult.OtherSymbols);
        }
    }
}
