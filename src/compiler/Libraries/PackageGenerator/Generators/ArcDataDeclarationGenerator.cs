using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal static class ArcDataDeclarationGenerator
    {
        public static IEnumerable<byte> Encode(this ArcDataDeclarationDescriptor decl, IEnumerable<ArcScopeTreeDataTypeNode> typeNodes)
        {
            // TODO: Use bitmask
            var dataType = typeNodes.First(x => x.Name == decl.Type.FullName);
            return [
                decl.MemoryStorageType == ArcMemoryStorageType.Value ? (byte)0x01 : (byte)0x00,
                .. BitConverter.GetBytes(decl.Dimension),
                decl.Mutability == ArcMutability.Variable ? (byte)0x01 : (byte)0x00,
                .. BitConverter.GetBytes(dataType.Id),
            ];
        }
    }
}
