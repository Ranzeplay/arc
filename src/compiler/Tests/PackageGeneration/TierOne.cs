using Arc.Compiler.PackageGenerator;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.Tests.PackageGeneration
{
    [CancelAfter(1000)]
    [Category("PackageGeneration")]
    internal class TierOne
    {
        [Test]
        public void EmptyFunction()
        {
            var text = "namespace Arc::Program { public func main(): val none {} }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text);
            var unit = new ArcCompilationUnit(compilationUnitContext, "test");
            var result = Flow.GenerateUnit(unit);

            Assert.That(result.Symbols, Has.Count.EqualTo(1));
        }
    }
}
