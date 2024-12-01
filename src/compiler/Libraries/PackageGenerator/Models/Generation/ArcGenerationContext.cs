using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    public class ArcGenerationContext(ArcPackageDescriptor packageDescriptor, IEnumerable<ArcCompilationUnit> compilationUnits)
    {
        public ArcPackageDescriptor PackageDescriptor { get; set; } = packageDescriptor;

        public IEnumerable<ArcCompilationUnit> CompilationUnits { get; set; } = compilationUnits;

        public IEnumerable<ArcRelocationLabel> Labels { get; set; } = [];

        public IEnumerable<ArcGenerationScope> Scopes { get; set; } = [];
    }
}
