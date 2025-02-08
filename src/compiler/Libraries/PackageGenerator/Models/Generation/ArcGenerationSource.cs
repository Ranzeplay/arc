using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    public class ArcGenerationSource
    {
        public ArcScopeTree GlobalScopeTree { get; set; }

        public IEnumerable<ArcScopeTreeNamespaceNode> LinkedNamespaces { get; set; } = [];

        public ArcScopeTreeNodeBase CurrentNode { get; set; }

        public List<ArcDataSlot> LocalDataSlots { get; set; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public ArcSignature ParentSignature { get; set; }

        public IEnumerable<ArcConstant> AccessibleConstants { get; set; } = [];

        public void Merge(ArcGenerationSource generationSource)
        {
            LocalDataSlots.AddRange(generationSource.LocalDataSlots);
            AccessibleConstants = AccessibleConstants.Concat(generationSource.AccessibleConstants);
        }

        public void Merge(ArcPartialGenerationResult generationResult)
        {
            LocalDataSlots.AddRange(generationResult.DataSlots);
            AccessibleConstants = AccessibleConstants.Concat(generationResult.AddedConstants);
        }
    }
}
