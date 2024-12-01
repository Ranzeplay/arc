using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors.Group
{
    public class ArcGroupFieldDescriptor : ArcSymbolBase, IArcLocatable
    {
        public string RawFullName => Name;

        public ArcDataDeclarationDescriptor DataType { get; set; }

        public IEnumerable<ArcAnnotationDescriptor> Annotations { get; set; }

        public ArcAccessibility Accessibility { get; set; }

        public string GetSignature() => $"D{RawFullName}";
    }
}
