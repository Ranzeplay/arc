using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors.Group
{
    public class ArcGroupDescriptor : ArcSymbolBase
    {
        public string ShortName { get; set; }

        public List<ArcFunctionDescriptor> Functions { get; set; } = [];

        public List<ArcFunctionDescriptor> Constructors { get; set; } = [];

        public List<ArcFunctionDescriptor> Destructors { get; set; } = [];

        public List<ArcGroupFieldDescriptor> Fields { get; set; } = [];

        public List<ArcGroupDescriptor> Groups { get; set; } = [];

        public List<ArcAnnotationDescriptor> Annotations { get; set; } = [];

        public ArcAccessibility Accessibility { get; set; } = ArcAccessibility.Private;
    }
}
