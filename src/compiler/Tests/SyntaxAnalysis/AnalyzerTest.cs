using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Arc.Compiler.Tests.SyntaxAnalysis
{
    [CancelAfter(1000)]
    [Category("SyntaxAnalysis")]
    internal class AnalyzerTest
    {
        private readonly ILogger _logger = LoggerFactory.Create(builder => { }).CreateLogger<AnalyzerTest>();

        [Test]
        public void AntlrParsing()
        {
            var text = Encoding.UTF8.GetString(Resource.test_script);
            var compilationUnit = AntlrAdapter.ParseCompilationUnit(text, _logger);

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
            var compilationUnit = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var transformed = new ArcCompilationUnit(compilationUnit, _logger, "test");

            Assert.That(transformed, Is.Not.Null);
        }
    }
}
