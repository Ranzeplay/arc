using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors.Function
{
    public class ArcFunctionDescriptor : ArcSymbolBase
    {
        public string RawFullName { get => Name; set => Name = value; }

        public IEnumerable<ArcParameterDescriptor> Parameters { get; set; }

        public ArcDataDeclarationDescriptor ReturnValueType { get; set; }

        public IEnumerable<ArcAnnotationDescriptor> Annotations { get; set; }

        public long EntrypointPos { get; set; }

        public long BlockLength { get; set; }

        public ArcAccessibility Accessibility { get; set; }

        public bool IsFunctionMatch(ArcFunctionCall functionCall)
        {
            var funcIdent = functionCall.Identifier.CloneWithoutContext();
            funcIdent.Name = funcIdent.Name.Insert(0, "F");

            return Name.StartsWith(funcIdent.ToString()) && functionCall.Arguments.Count() == Parameters.Count();
        }
    }
}
