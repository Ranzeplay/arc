using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeAnnotationNode : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode
    {
        public override string Name => TargetGroup.ShortName;

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Annotation;

        public required ArcScopeTreeGroupNode TargetGroup { get; set; }

        public override string SignatureAddend => "A" + TargetGroup.ShortName;

        public IEnumerable<byte> Encode(ArcScopeTree tree) =>
            [
                (byte)ArcSymbolType.Annotation,
                ..BitConverter.GetBytes(TargetGroup.Id)
            ];
    }
}
