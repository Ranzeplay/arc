using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal static class ArcGroupSymbolEncoder
    {
        public static IEnumerable<byte> EncodeGroupSymbol(ArcScopeTreeGroupNode group)
        {
            return [
                // Field id list
                .. ArcArrayEncoder.SerializeArray(group.Fields.Select(field => field.Id)),
                // Lifecycle function id list
                .. BitConverter.GetBytes(group.LifecycleFunctions.LongCount()),
                .. group.LifecycleFunctions.SelectMany(f => (byte[]) [(byte)f.SyntaxTree.LifecycleStage, .. BitConverter.GetBytes(f.Id)]),
                // Function id list
                .. ArcArrayEncoder.SerializeArray(group.Functions.Select(function => function.Id)),
                // Subgroup id list
                .. ArcArrayEncoder.SerializeArray(group.Groups.Select(subGroup => subGroup.Id)),
                // Annotation id list
                .. ArcArrayEncoder.SerializeArray(group.Annotations.Select(annotation => annotation.Key.Id)),
                // Generic id list
                .. ArcArrayEncoder.SerializeArray(group.GenericTypes.Select(g => g.Id)),
                // Derivation id list
                .. ArcArrayEncoder.SerializeArray(group.Derivations.Select(derivation => derivation.Target.ProxyTypeId)),
            ];
        }
    }
}
