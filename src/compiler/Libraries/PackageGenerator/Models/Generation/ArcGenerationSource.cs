using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    public class ArcGenerationSource
    {
        public WeakReference<ArcGenerationContext> ContextReference { get; }

        public IEnumerable<ArcDataSlot> LocalDataSlots { get; set; } = [];

        public IEnumerable<ArcSymbolBase> AccessibleSymbols { get; set; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public ArcSignature ParentSignature { get; set; }

        public IEnumerable<ArcConstant> AccessibleConstants { get; set; } = [];

        public void Merge(ArcGenerationSource generationSource)
        {
            LocalDataSlots = LocalDataSlots.Concat(generationSource.LocalDataSlots);
            AccessibleSymbols = AccessibleSymbols.Concat(generationSource.AccessibleSymbols);
            AccessibleConstants = AccessibleConstants.Concat(generationSource.AccessibleConstants);
        }

        public void Merge(ArcPartialGenerationResult generationResult)
        {
            LocalDataSlots = LocalDataSlots.Concat(generationResult.DataSlots);
            AccessibleSymbols = AccessibleSymbols.Concat(generationResult.OtherSymbols);
            AccessibleConstants = AccessibleConstants.Concat(generationResult.AddedConstants);
        }
    }
}
