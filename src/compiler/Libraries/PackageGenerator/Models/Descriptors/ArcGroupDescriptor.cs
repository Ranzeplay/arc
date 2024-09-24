using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcGroupDescriptor
    {
        public long Id { get; set; }

        public string RawFullName { get; set; }

        public IEnumerable<ArcFunctionDescriptor> Functions { get; set; }

        public IEnumerable<ArcFunctionDescriptor> Constructors { get; set; }

        public IEnumerable<ArcFunctionDescriptor> Destructors { get; set; }

        public IEnumerable<ArcGroupFieldDescriptor> Fields { get; set; }

        public IEnumerable<ArcGroupDescriptor> Groups { get; set; }

        public IEnumerable<ArcAnnotationDescriptor> Annotations { get; set; }

        public ArcAccessibility Accessibility { get; set; }
    }
}
