using System.Text;
using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Encoders;

internal static class ArcGinkgoPackageEncoder
{
    public static byte[] Encode(ArcPackageDescriptor descriptor, List<ArcScopeTreeNodeBase> symbols,
        Dictionary<uint, uint> functionPools, IReadOnlyList<ArcGinkgoUnitGenerationContext> contexts, byte[] code)
    {
        using var output = new MemoryStream();
        using var writer = new BinaryWriter(output, Encoding.UTF8, leaveOpen: true);
        writer.Write("GINKGO\0\0"u8);
        writer.Write(2u);
        WriteDescriptor();

        // Keep the legacy order: descriptor, counted symbol table, counted constants, code.
        var encodedSymbols = symbols.Where(n => n is ArcScopeTreeNamespaceNode or ArcScopeTreeGroupFieldNode
            or ArcScopeTreeDataTypeNode or ArcScopeTreeGroupNode or ArcScopeTreeAnnotationNode
            or ArcScopeTreeEnumNode or ArcScopeTreeEnumMemberNode or ArcScopeTreeGenericTypeNode ||
            n is ArcScopeTreeFunctionNodeBase && functionPools.ContainsKey(n.Id)).ToList();
        writer.Write(checked((uint)encodedSymbols.Count));
        foreach (var symbol in encodedSymbols)
        {
            writer.Write(symbol.Id);
            switch (symbol)
            {
                case ArcScopeTreeFunctionNodeBase function:
                    writer.Write((byte)ArcSymbolType.Function);
                    writer.Write(function.EntrypointPos);
                    writer.Write(function.BlockLength);
                    WriteDeclaration(function.ReturnValueType);
                    writer.Write(checked((uint)function.Parameters.Count()));
                    foreach (var parameter in function.Parameters) WriteDeclaration(parameter.DataType);
                    WriteIds(function.Annotations.Keys.Select(a => AnnotationId(a.Signature)));
                    WriteIds(function.GenericTypes.Select(g => g.Id));
                    writer.Write(function.DataCount);
                    writer.Write(functionPools[function.Id]);
                    break;
                case ArcScopeTreeGroupFieldNode field:
                    writer.Write((byte)ArcSymbolType.GroupField);
                    WriteDeclaration(field.DataType);
                    break;
                case ArcScopeTreeDataTypeNode type:
                    writer.Write((byte)ArcSymbolType.DataType);
                    writer.Write((byte)(type.IsInternal ? 0 : 1));
                    if (!type.IsInternal) writer.Write(type.ComplexTypeGroup?.Id ?? type.EnumType!.Id);
                    break;
                case ArcScopeTreeGroupNode group:
                    writer.Write((byte)ArcSymbolType.Group);
                    WriteIds(group.Fields.Select(f => f.Id));
                    var lifecycleFunctions = group.LifecycleFunctions.Where(f => functionPools.ContainsKey(f.Id)).ToList();
                    writer.Write(checked((uint)lifecycleFunctions.Count));
                    foreach (var function in lifecycleFunctions)
                    {
                        writer.Write((byte)function.SyntaxTree.LifecycleStage);
                        writer.Write(function.Id);
                    }
                    WriteIds(group.Functions.Where(f => functionPools.ContainsKey(f.Id)).Select(f => f.Id));
                    WriteIds(group.Groups.Select(g => g.Id));
                    WriteIds(group.Annotations.Keys.Select(a => a.Id));
                    WriteIds(group.GenericTypes.Select(g => g.Id));
                    WriteIds(group.Derivations.Select(d => symbols.OfType<ArcScopeTreeDataTypeNode>()
                        .First(t => t.Signature == d.Target.TypeSignature).Id));
                    break;
                case ArcScopeTreeNamespaceNode:
                    writer.Write((byte)ArcSymbolType.Namespace);
                    break;
                case ArcScopeTreeAnnotationNode annotation:
                    writer.Write((byte)ArcSymbolType.Annotation);
                    writer.Write(annotation.TargetGroup.Id);
                    break;
                case ArcScopeTreeEnumNode enumeration:
                    writer.Write((byte)ArcSymbolType.Enum);
                    WriteIds(enumeration.Annotations.Keys.Select(a => AnnotationId(a.Signature)));
                    WriteIds(enumeration.Members.Select(m => m.Id));
                    break;
                case ArcScopeTreeEnumMemberNode member:
                    writer.Write((byte)ArcSymbolType.EnumMember);
                    WriteIds(member.Annotations.Keys.Select(a => AnnotationId(a.Signature)));
                    break;
                case ArcScopeTreeGenericTypeNode generic:
                    writer.Write((byte)ArcSymbolType.GenericTypeParameter);
                    writer.Write(symbols.OfType<ArcScopeTreeDataTypeNode>()
                        .First(t => t.Signature == generic.ConstraintType.Signature).Id);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported Ginkgo symbol: {symbol.NodeType}");
            }
        }

        writer.Write(checked((uint)contexts.Count));
        foreach (var context in contexts)
        {
            writer.Write(checked((uint)context.Constants.Count));
            for (var index = 0; index < context.Constants.Count; index++)
            {
                var constant = context.Constants[index];
                writer.Write((uint)index);
                writer.Write(checked((uint)constant.TypeId));
                writer.Write(false); // Scalar, as in the legacy constant descriptor.
                byte[] data = constant.Value switch
                {
                    long integer => BitConverter.GetBytes(integer),
                    decimal number => BitConverter.GetBytes((double)number),
                    bool boolean => [boolean ? (byte)1 : (byte)0],
                    string text => Encoding.UTF8.GetBytes(text),
                    byte[] bytes => bytes,
                    _ => throw new InvalidOperationException("Unsupported Ginkgo constant type")
                };
                writer.Write((ulong)data.LongLength);
                writer.Write(data);
            }
        }
        writer.Write(code);
        return output.ToArray();

        void WriteIds(IEnumerable<uint> ids) => writer.Write(ArcArrayEncoder.SerializeArray(ids).ToArray());

        uint AnnotationId(string signature) => symbols.First(n => n.Signature == signature).Id;

        void WriteDescriptor()
        {
            writer.Write((byte)descriptor.Type);
            var nameBytes = Encoding.UTF8.GetBytes(descriptor.Name);
            writer.Write((ulong)nameBytes.LongLength);
            writer.Write(nameBytes);
            writer.Write(descriptor.Version);
            writer.Write(checked((uint)descriptor.EntrypointFunctionId));
            writer.Write(descriptor.DataAlignmentLength);
            writer.Write(descriptor.RootFunctionTableEntryPos);
            writer.Write(descriptor.RootConstantTableEntryPos);
            writer.Write(descriptor.RootGroupTableEntryPos);
            writer.Write(descriptor.RegionTableEntryPos);
        }

        void WriteDeclaration(ArcDataDeclarationDescriptor declaration)
        {
            var type = symbols.OfType<ArcScopeTreeDataTypeNode>()
                .First(t => t.ResolvedType.TypeId == declaration.Type.TypeId);
            writer.Write(declaration.Dimension);
            writer.Write((byte)declaration.Mutability);
            writer.Write(type.Id);
            writer.Write(declaration.AllowNone);
        }
    }
}
