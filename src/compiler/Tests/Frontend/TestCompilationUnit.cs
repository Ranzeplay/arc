using Arc.Compiler.SyntaxAnalyzer;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.Frontend;

[TestFixture]
[CancelAfter(1000)]
[Category("Frontend")]
public class TestCompilationUnit
{
    private string SourceCode { get; set; } = string.Empty;
    private readonly ILogger _logger = LoggerFactory.Create(_ => { }).CreateLogger<TestCompilationUnit>();

    [SetUp]
    public void SetUp()
    {
        SourceCode = ResourceHandler.LoadResourceAsString("helloWorld.script.arc");
    }

    [Test]
    public void Parse()
    {
        var compilationUnit = AntlrAdapter.ParseCompilationUnit(SourceCode, _logger);
        Assert.That(compilationUnit, Is.Not.Null);
    }
}