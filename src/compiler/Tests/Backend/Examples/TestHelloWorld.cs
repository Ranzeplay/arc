using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Arc.Compiler.Tests.Frontend;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.Backend.Examples;

[TestFixture]
public class TestHelloWorld
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
        var unit = new ArcCompilationUnit(compilationUnit, _logger, "helloWorld");
        var context = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Executable));
        
        Assert.That(context, Is.Not.Null);
    }
}