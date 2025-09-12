using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    public class ArcDataDeclarationDescriptor
    {
        public required ArcTypeBase Type { get; set; }

        public int Dimension { get; set; }

        public bool AllowNone { get; set; }

        public ArcMutability Mutability { get; set; }

        public ArcDataDeclarator SyntaxTree { get; set; }
    }
}
