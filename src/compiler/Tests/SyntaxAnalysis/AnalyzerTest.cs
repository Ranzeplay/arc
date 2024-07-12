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
            var text = "link arc::std; namespace Class {}";
            var compilationUnit = ANTLRAdapter.ParseCompilationUnit(text);

            Assert.That(compilationUnit, Is.Not.Null);

            Assert.That(
                compilationUnit
                    .arc_stmt_link()
                    .First()
                    .arc_namespace_identifier()
                    .IDENTIFIER()
                    .First()
                    .GetText(), 
                Is.EqualTo("arc")
            );
        }
    }
}
