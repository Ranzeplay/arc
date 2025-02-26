using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

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

        public Dictionary<ArcAnnotationDescriptor, IEnumerable<ArcExpression>> Annotations { get; set; } = [];

        public ArcAccessibility Accessibility { get; set; } = ArcAccessibility.Private;
    }
}
