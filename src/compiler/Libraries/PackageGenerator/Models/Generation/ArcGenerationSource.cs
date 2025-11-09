using Arc.Compiler.PackageGenerator.Interfaces;
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

        public IEnumerable<IArcDataTypeProxy> DirectlyAccessibleTypes
        {
            get
            {
                var result = LinkedNamespaces.SelectMany(lns => lns.Children).OfType<IArcDataTypeProxy>();
                if (CurrentNode is ArcScopeTreeFunctionNodeBase functionNodeBase)
                {
                    result = result.Concat(functionNodeBase.GenericTypes);

                    if (CurrentNode.Parent is ArcScopeTreeGroupNode groupNode)
                    {
                        result = result.Concat(groupNode.GenericTypes);
                    }
                }

                return result;
            }
        }

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

        public ArcGenerationSource Clone()
        {
            return (ArcGenerationSource) MemberwiseClone();
        }
    }
}
