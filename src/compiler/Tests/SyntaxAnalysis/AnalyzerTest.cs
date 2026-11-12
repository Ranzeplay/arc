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
        private readonly ILogger _logger = LoggerFactory.Create(_ => { }).CreateLogger<AnalyzerTest>();

        [Test]
        public void InvalidQualifiedCallsReportParserErrorsInsteadOfReturningARecoveredTree()
        {
            var exception = Assert.Throws<ArcSyntaxException>(() => AntlrAdapter.ParseCompilationUnit(
                "namespace Test { public func main(): int { return Test::answer(); } }", _logger, "invalid.arc"));

            Assert.Multiple(() =>
            {
                Assert.That(exception!.Diagnostics, Has.Count.EqualTo(2));
                Assert.That(exception.Diagnostics[0], Does.StartWith("invalid.arc (1,55):"));
                Assert.That(exception.Diagnostics[0], Does.Contain("mismatched input '::'"));
                Assert.That(exception.Message, Does.Contain(exception.Diagnostics[1]));
            });
        }

        [Test]
        public void LexerErrorsAreReportedEvenWhenTheRemainingTokensAreValid()
        {
            var exception = Assert.Throws<ArcSyntaxException>(() => AntlrAdapter.ParseCompilationUnit(
                "namespace Test {\n$\n}", _logger, "invalid.arc"));

            Assert.That(exception!.Diagnostics.Single(),
                Is.EqualTo("invalid.arc (2,1): token recognition error at: '$'"));
        }

        [Test]
        public void QualifiedCallsUsingNamespaceLimitersParseWithoutErrors()
        {
            Assert.DoesNotThrow(() => AntlrAdapter.ParseCompilationUnit(
                "namespace Test { public func main(): int { return [Test].answer(); } }", _logger));
        }

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

            Assert.That(
                transformed.Namespace.Functions.First().Body,
                Is.Not.Null
            );
        }
    }
}
