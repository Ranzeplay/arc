using Arc.Compiler.PackageGenerator.Encoders;
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

        public ArcPartialGenerationResult GenerationResult { get; set; }

        public virtual IEnumerable<byte> Encode(ArcScopeTree tree)
        {
            var iterResult = new List<byte>();

            iterResult.Add((byte)ArcSymbolType.Function);
            iterResult.AddRange(BitConverter.GetBytes(EntrypointPos));
            iterResult.AddRange(BitConverter.GetBytes(BlockLength));
            iterResult.AddRange(new ArcStringEncoder().Encode(Signature));

            // Return type descriptor
            iterResult.AddRange(ReturnValueType.Encode(tree.GetNodes<ArcScopeTreeDataTypeNode>()));

            // Parameter type descriptors
            iterResult.AddRange(BitConverter.GetBytes(Parameters.LongCount()));
            foreach (var parameter in Parameters)
            {
                iterResult.AddRange(parameter.DataType.Encode(tree.GetNodes<ArcScopeTreeDataTypeNode>()));
            }

            // Annotation descriptors
            iterResult.AddRange(BitConverter.GetBytes(Annotations.LongCount()));
            foreach (var annotation in Annotations)
            {
                iterResult.AddRange(BitConverter.GetBytes(annotation.Key.Id));
            }

            return iterResult;
        }
    }
}
