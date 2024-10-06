using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors.Function
{
    internal class ArcFunctionDescriptor : ArcSymbolBase
    {
        public string RawFullName { get => Name; set => Name = value; }

        public IEnumerable<ArcParameterDescriptor> Parameters { get; set; }

        public ArcDataDeclarationDescriptor ReturnValueType { get; set; }

        public IEnumerable<ArcAnnotationDescriptor> Annotations { get; set; }

        public long EntrypointPos { get; set; }

        public long BlockLength { get; set; }

        public ArcAccessibility Accessibility { get; set; }
    }
}
