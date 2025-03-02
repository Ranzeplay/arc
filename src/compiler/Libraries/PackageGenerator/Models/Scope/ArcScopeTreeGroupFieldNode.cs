using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public class ArcScopeTreeGroupFieldNode : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.GroupField;

        public required ArcDataDeclarationDescriptor DataType { get; set; }

        public Dictionary<ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];

        public required ArcAccessibility Accessibility { get; set; }

        public required string IdentifierName { get; set; }

        public override string SignatureAddend => "D" + IdentifierName;

        public override string Name => IdentifierName;

        public IEnumerable<byte> Encode(ArcScopeTree tree) =>
            [
                (byte)ArcSymbolType.GroupField,
                ..new ArcStringEncoder().Encode(Signature),
                ..DataType.Encode(tree.GetNodes<ArcScopeTreeDataTypeNode>())
            ];
    }
}
