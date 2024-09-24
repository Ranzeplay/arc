using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcGroupFieldDescriptor
    {
        public long Index { get; set; }

        public string RawFullName { get; set; }

        public ArcDataTypeDescriptor DataType { get; set; }

        public IEnumerable<ArcAnnotationDescriptor> Annotations { get; set; }

        public ArcAccessibility Accessibility { get; set; }
    }
}
