using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcGroupSymbolEncoder
    {
        public static IEnumerable<byte> EncodeGroupSymbol(ArcScopeTreeGroupNode group)
        {
            return [
                // Field id list
                ..BitConverter.GetBytes((long)group.Fields.Count),
                ..group.Fields.SelectMany(field => BitConverter.GetBytes(field.Id)),
                // Constructor id list
                ..BitConverter.GetBytes((long) group.Constructors.Count),
                ..group.Constructors.SelectMany(constructor => BitConverter.GetBytes(constructor.Id)),
                // Destructor id list
                ..BitConverter.GetBytes((long) group.Destructors.Count),
                ..group.Destructors.SelectMany(destructor => BitConverter.GetBytes(destructor.Id)),
                // Function id list
                ..BitConverter.GetBytes((long) group.Functions.Count),
                ..group.Functions.SelectMany(function => BitConverter.GetBytes(function.Id)),
                // Subgroup id list
                ..BitConverter.GetBytes((long) group.Groups.Count),
                ..group.Groups.SelectMany(subGroup => BitConverter.GetBytes(subGroup.Id)),
                // Annotation id list
                ..BitConverter.GetBytes((long) group.Annotations.Count),
                ..group.Annotations.SelectMany(annotation => BitConverter.GetBytes(annotation.Key.Id)),
                // Generic id list
                ..BitConverter.GetBytes(group.GenericTypes.LongCount()),
                ..group.GenericTypes.SelectMany(g => BitConverter.GetBytes(g.Id)),
            ];
        }
    }
}
