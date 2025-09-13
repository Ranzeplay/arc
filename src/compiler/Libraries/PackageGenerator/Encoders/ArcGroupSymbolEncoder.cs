using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcGroupSymbolEncoder
    {
        public static IEnumerable<byte> EncodeGroupSymbol(ArcScopeTreeGroupNode group)
        {
            return [
                // Field id list
                ..BitConverter.GetBytes(group.Fields.LongCount()),
                ..group.Fields.SelectMany(field => BitConverter.GetBytes(field.Id)),
                // Lifecycle function id list
                ..BitConverter.GetBytes(group.LifecycleFunctions.LongCount()),
                ..group.LifecycleFunctions.SelectMany(f => (byte[]) [(byte)f.SyntaxTree.LifecycleStage, .. BitConverter.GetBytes(f.Id)]),
                // Function id list
                ..BitConverter.GetBytes(group.Functions.LongCount()),
                ..group.Functions.SelectMany(function => BitConverter.GetBytes(function.Id)),
                // Subgroup id list
                ..BitConverter.GetBytes(group.Groups.LongCount()),
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
