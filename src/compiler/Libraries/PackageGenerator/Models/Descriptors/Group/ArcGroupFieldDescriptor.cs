using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors.Group
{
    public class ArcGroupFieldDescriptor : ArcSymbolBase, IArcLocatable
    {
        public string RawFullName => Name;

        public required ArcDataDeclarationDescriptor DataType { get; set; }

        public Dictionary<ArcAnnotationDescriptor, IEnumerable<ArcExpression>> Annotations { get; set; } = [];

        public ArcAccessibility Accessibility { get; set; }

        public required string IdentifierName { get; set; }

        public string GetSignature() => $"D{RawFullName}";
    }
}
