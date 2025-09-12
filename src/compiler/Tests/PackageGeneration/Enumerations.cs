using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration;

[Category("PackageGeneration")]
[CancelAfter(1000)]
internal class Enumerations
{
    private readonly ILogger _logger = LoggerFactory.Create(_ => { }).CreateLogger<Enumerations>();
    
    [Test]
    public void Definition()
    {
        const string text = """
                            namespace Arc::Lib {
                                public enum Status {
                                    ONLINE,
                                    BUSY,
                                    OFFLINE
                                }
                            }
                            """;
        
        var compilationUnit = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var syntaxUnit = new ArcCompilationUnit(compilationUnit, _logger, "test");
        var context = ArcCombinedUnitGenerator.GenerateUnits([syntaxUnit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        var outputStream = context.DumpFullByteStream();
        Assert.That(outputStream, Is.Not.Null);
    }
    
    [Test]
    public void DefinitionAndUsage()
    {
        const string text = """
                            namespace Arc::Lib {
                                public enum Status {
                                    ONLINE,
                                    BUSY,
                                    OFFLINE
                                }
                                
                                public func test(): none {
                                    var status: Status;
                                    status = Status.ONLINE;
                                }
                            }
                            """;
        
        var compilationUnit = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var syntaxUnit = new ArcCompilationUnit(compilationUnit, _logger, "test");
        var context = ArcCombinedUnitGenerator.GenerateUnits([syntaxUnit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        var outputStream = context.DumpFullByteStream();
        Assert.That(outputStream, Is.Not.Null);
    }
}
