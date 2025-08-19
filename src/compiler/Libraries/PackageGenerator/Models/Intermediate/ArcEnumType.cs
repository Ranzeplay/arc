using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    public class ArcEnumType(ArcScopeTreeEnumNode enumDescriptor) : ArcTypeBase(enumDescriptor.Name)
    {
        public ulong EnumId { get; set; } = enumDescriptor.Id;
    }
}
