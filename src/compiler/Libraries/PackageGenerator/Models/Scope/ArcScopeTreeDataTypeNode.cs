using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeDataTypeNode(ArcTypeBase dataType, string shortName) : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.DataType;

        public bool IsInternal { get; set; } = dataType is ArcBaseType;

        public ArcTypeBase DataType { get; set; } = dataType;

        public string ShortName { get; set; } = shortName;

        public ArcScopeTreeGroupNode? ComplexTypeGroup { get; set; }

        public override string SignatureAddend => "T" + ShortName;

        public override string Name => DataType.Identifier;

        public IEnumerable<byte> Encode(ArcScopeTree tree) =>
            [
                (byte)ArcSymbolType.DataType,
                (byte)(IsInternal ? 0x00 : 0x01),
                ..new ArcStringEncoder().Encode(Signature),
                ..(!IsInternal ? BitConverter.GetBytes(ComplexTypeGroup?.Id ?? 0xffffffff) : [])
            ];

        public static ArcScopeTreeDataTypeNode Placeholder() => new(ArcBaseType.Placeholder(), "INVALID");
    }
}
