using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal static class ArcDataDeclarationGenerator
    {
        public static IEnumerable<byte> Encode(this ArcDataDeclarationDescriptor decl, IEnumerable<ArcSymbolBase> symbols)
        {
            // TODO: Use bitmask
            var dataType = symbols.First(x => x is ArcTypeBase && x.Name == decl.Type.FullName);
            return [
                decl.MemoryStorageType == ArcMemoryStorageType.Value ? (byte)0x01 : (byte)0x00,
                decl.IsArray ? (byte)0x01 : (byte)0x00,
                decl.Mutability == ArcMutability.Variable ? (byte)0x01 : (byte)0x00,
                .. BitConverter.GetBytes(dataType.Id),
            ];
        }
    }
}
