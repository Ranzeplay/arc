using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeDataTypeNode(ArcDataTypeType typeType, ArcTypeBase dataType, string shortName) : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode, IArcDataTypeProxy
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.DataType;

        public bool IsInternal { get; set; } = dataType is ArcBaseType;

        public ArcDataTypeType ArcDataTypeType { get; set; } = typeType;

        public ArcTypeBase DataType { get; set; } = dataType;

        public string ShortName { get; set; } = shortName;

        public ArcScopeTreeGroupNode? ComplexTypeGroup { get; set; }

        public ArcScopeTreeEnumNode? EnumGroup { get; set; }

        public override string SignatureAddend => "T" + ShortName;

        public override string Name => DataType.Identifier;

        public string TypeSignature => Signature;

        public ulong ProxyTypeId => Id;

        public ulong ActualTypeId => Id;

        public ArcTypeBase ResolvedType => DataType;

        public IEnumerable<byte> Encode(ArcScopeTree tree) =>
            [
                (byte)ArcSymbolType.DataType,
                (byte)(IsInternal ? 0x00 : 0x01),
                ..(!IsInternal ? BitConverter.GetBytes(ComplexTypeGroup?.Id ?? 0xffffffff) : [])
            ];

        public static ArcScopeTreeDataTypeNode Placeholder() => new(ArcDataTypeType.Primitive, ArcBaseType.Placeholder(), "INVALID");
    }
}
