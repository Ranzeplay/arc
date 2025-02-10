using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcDataSlot : ArcSymbolBase
    {
        public ArcDataDeclarationDescriptor DeclarationDescriptor { get; set; }

        public long SlotId { get; set; }
    }
}
