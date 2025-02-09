using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ArcComplexType(ArcGroupDescriptor groupDescriptor) : ArcTypeBase(groupDescriptor.Name)
    {
        public long GroupId { get; set; } = groupDescriptor.Id;
    }
}
