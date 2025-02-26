using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors.Function
{
    public class ArcFunctionDescriptor : ArcSymbolBase
    {
        public string RawFullName { get => Name; set => Name = value; }

        public IEnumerable<ArcParameterDescriptor> Parameters { get; set; } = [];

        public ArcDataDeclarationDescriptor ReturnValueType { get; set; }

        public Dictionary<ArcAnnotationDescriptor, IEnumerable<ArcExpression>> Annotations { get; set; } = [];

        public long EntrypointPos { get; set; }

        public long BlockLength { get; set; }

        public ArcAccessibility Accessibility { get; set; }
    }
}
