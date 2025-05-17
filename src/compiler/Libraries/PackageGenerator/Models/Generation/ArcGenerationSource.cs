using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    public class ArcGenerationSource
    {
        public required string Name { get; set; }

        public ArcScopeTree GlobalScopeTree { get; set; }

        public IEnumerable<ArcScopeTreeNamespaceNode> LinkedNamespaces { get; set; } = [];

        public IEnumerable<ArcScopeTreeGenericTypeNode> GenericTypes { get; set; } = [];

        public IEnumerable<ArcScopeTreeNodeBase> DirectlyAccessibleNodes => LinkedNamespaces.SelectMany(lns => lns.Children);

        public ArcScopeTreeNodeBase CurrentNode { get; set; }

        public List<ArcDataSlot> LocalDataSlots { get; set; } = [];

        public required ArcPackageDescriptor PackageDescriptor { get; set; }

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
