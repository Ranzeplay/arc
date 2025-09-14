using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeGenericTypeNode : ArcScopeTreeNodeBase, IArcDataTypeProxy, IArcEncodableScopeTreeNode
    {
        public override string Name => Identifier;

        public required string Identifier { get; set; }

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GenericType;

        public ArcScopeTreeDataTypeNode ConstraintType { get; set; } = ArcPersistentData.BaseTypeScopeTree.GetNodeByName<ArcScopeTreeDataTypeNode>("any")!;

        public override string SignatureAddend => "P" + Name;

        public string TypeSignature => Signature;

        public ulong ProxyTypeId => ConstraintType.Id;

        public ulong ActualTypeId => Id;

        public string ShortName => Identifier;

        public ArcTypeBase ResolvedType => ConstraintType.ResolvedType;
        
        public IEnumerable<byte> Encode(ArcScopeTree tree) =>
        [
            (byte) ArcSymbolType.DataType,
            0x01, // Non-internal type
            ..BitConverter.GetBytes(ProxyTypeId)
        ];
    }
}
