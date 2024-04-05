using Arc.Compiler.SyntaxAnalyzer;

namespace Arc.Compiler.Tests.SyntaxAnalysis
{
    [CancelAfter(1000)]
    [Category("SyntaxAnalysis")]
    internal class AnalyzerTest
    {
        [Test]
        public void Test()
        {
            var text = "link anb::def;";
            var compilationUnit = ANTLRAdapter.ParseCompilationUnit(text);

            Assert.That(compilationUnit, Is.Not.Null);
        }
    }
}
