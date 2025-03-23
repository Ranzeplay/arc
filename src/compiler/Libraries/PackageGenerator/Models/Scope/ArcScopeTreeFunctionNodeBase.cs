using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public abstract class ArcScopeTreeFunctionNodeBase : ArcScopeTreeNodeBase, IArcEncodableScopeTreeNode
    {
        public IEnumerable<ArcParameterDescriptor> Parameters { get; set; } = [];

        public ArcDataDeclarationDescriptor ReturnValueType { get; set; }

        public Dictionary<ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];

        public ArcAccessibility Accessibility { get; set; }

        public long EntrypointPos { get; set; }

        public long BlockLength { get; set; }

        public long DataCount { get; set; }

        public ArcPartialGenerationResult GenerationResult { get; set; }

        public virtual IEnumerable<byte> Encode(ArcScopeTree tree) =>
            [
                (byte)ArcSymbolType.Function,
                ..BitConverter.GetBytes(EntrypointPos),
                ..BitConverter.GetBytes(BlockLength),

                ..ReturnValueType.Encode(tree.GetNodes<ArcScopeTreeDataTypeNode>()),

                ..BitConverter.GetBytes(Parameters.LongCount()),
                ..Parameters.SelectMany(p => p.DataType.Encode(tree.GetNodes<ArcScopeTreeDataTypeNode>())),

                ..BitConverter.GetBytes(Annotations.LongCount()),
                ..Annotations.SelectMany(a => BitConverter.GetBytes(a.Key.Id)),

                ..BitConverter.GetBytes(DataCount)
            ];
    }
}
