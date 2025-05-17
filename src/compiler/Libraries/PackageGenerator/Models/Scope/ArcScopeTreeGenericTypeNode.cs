using Arc.Compiler.PackageGenerator.Models.Builtin;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeGenericTypeNode : ArcScopeTreeNodeBase
    {
        public override string Name => Identifier;

        public required string Identifier { get; set; }

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GenericType;

        public ArcScopeTreeDataTypeNode ResolvedType { get; set; } = ArcPersistentData.BaseTypeScopeTree.GetNodeByName<ArcScopeTreeDataTypeNode>("any")!;

        public override string SignatureAddend => "P" + Name;
    }
}
