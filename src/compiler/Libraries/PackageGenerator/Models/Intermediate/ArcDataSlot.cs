using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcDataSlot
    {
        public ArcDataDeclarationDescriptor DeclarationDescriptor { get; set; }

        public ulong SlotId { get; set; }

        public string Name { get; set; }
    }
}
