using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ArcComplexType(ArcScopeTreeGroupNode groupDescriptor) : ArcTypeBase(groupDescriptor.Name)
    {
        public long GroupId { get; set; } = groupDescriptor.Id;
    }
}
