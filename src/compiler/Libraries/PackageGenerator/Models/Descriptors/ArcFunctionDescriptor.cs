using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcFunctionDescriptor
    {
        public long Id { get; set; }

        public string RawFullName { get; set; }

        public IEnumerable<ArcParameterDescriptor> Parameters { get; set; }

        public ArcDataTypeDescriptor ReturnValueType { get; set; }

        public IEnumerable<ArcAnnotationDescriptor> Annotations { get; set; }

        public long EntrypointPos { get; set; }

        public long BlockLength { get; set; }

        public ArcAccessibility Accessibility { get; set; }
    }
}
