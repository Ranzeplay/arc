using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using System.Text;

namespace Arc.Compiler.Tests.SyntaxAnalysis
{
    [CancelAfter(1000)]
    [Category("SyntaxAnalysis")]
    internal class AnalyzerTest
    {
        [Test]
        public void AntlrParsing()
        {
            var text = Encoding.UTF8.GetString(Resource.test_script);
            var compilationUnit = AntlrAdapter.ParseCompilationUnit(text);

            Assert.That(compilationUnit, Is.Not.Null);

            Assert.That(
                compilationUnit
                    .arc_stmt_link()
                    .First()
                    .arc_namespace_identifier()
                    .IDENTIFIER()
                    .First()
                    .GetText(),
                Is.EqualTo("Arc")
            );
        }

        [Test]
        public void Transformation()
        {
            var text = Encoding.UTF8.GetString(Resource.test_script);
            var compilationUnit = AntlrAdapter.ParseCompilationUnit(text);
            var transformed = new ArcCompilationUnit(compilationUnit, "test");

            Assert.That(transformed, Is.Not.Null);
        }
    }
}
