using Arc.Compiler.SyntaxAnalyzer;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.Frontend;

[TestFixture]
[CancelAfter(1000)]
[Category("Frontend")]
[Category("Examples")]
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
        Assert.That(compilationUnit.exception, Is.Null);
    }
    
    [Test]
    public void ParseEmpty()
    {
        var compilationUnit = AntlrAdapter.ParseCompilationUnit(string.Empty, _logger);
        Assert.That(compilationUnit, Is.Not.Null);
        Assert.That(compilationUnit.exception, Is.Not.Null);
    }
}