using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration;

[Category("PackageGeneration")]
public class Generics
{
    private readonly ILogger _logger = LoggerFactory.Create(builder => { }).CreateLogger<Generics>();
    
    [Test]
    public void GenericsOnFunctionDeclarator()
    {
        const string text = """
                            namespace Arc::Lib { 
                                public func main<T>(): val int { 
                                    return 0;
                                }
                            }
                            """;
        
        var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
        var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GenericsOnFunctionParameters()
    {
        const string text = """
                            namespace Arc::Lib { 
                                public func main<T>(const t: val T): val int { 
                                    return 0;
                                }
                            }
                            """;
        
        var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
        var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        
        Assert.That(result, Is.Not.Null);
    }
    
    [Test]
    public void GenericsOnFunctionReturnType()
    {
        const string text = """
                            namespace Arc::Lib { 
                                public func main<T>(const t: val T): val T { 
                                    return t;
                                }
                            }
                            """;
        
        var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
        var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        
        Assert.That(result, Is.Not.Null);
    }
    
    [Test]
    public void GenericsOnDataDeclarator()
    {
        const string text = """
                            namespace Arc::Lib { 
                                public func main<T>(const t: val T): val int {
                                    const i: val T;
                                    i = t;
                                
                                    return 0;
                                }
                            }
                            """;
        
        var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
        var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        
        Assert.That(result, Is.Not.Null);
    }
    
    [Test]
    public void GenericsOnGroupDeclarator()
    {
        const string text = """
                            namespace Arc::Lib { 
                                public group G<T>
                                {
                                }
                            }
                            """;
        
        var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
        var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        
        Assert.That(result, Is.Not.Null);
    }
    
    [Test]
    public void GenericsOnGroupField()
    {
        const string text = """
                            namespace Arc::Lib { 
                                public group G<T>
                                {
                                    private field var item: val T;
                                }
                            }
                            """;
        
        var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
        var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        
        Assert.That(result, Is.Not.Null);
    }
    
    [Test]
    public void GenericsOnGroupFunction()
    {
        const string text = """
                            namespace Arc::Lib { 
                                public group G<T>
                                {
                                    private field var item: val T;
                                    
                                    public func getFoo(var self: ref G): val T
                                    {
                                        const i: val T;
                                        i = self.item;
                                    
                                    	return i;
                                    }
                                }
                            }
                            """;
        
        var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
        var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        
        Assert.That(result, Is.Not.Null);
    }
    
    [Test]
    public void NestedGenericsOnGroupFunction()
    {
        const string text = """
                            namespace Arc::Lib { 
                                public group G<T>
                                {
                                    public func getFoo<U>(var u: val U, var t: val T): val U
                                    {
                                    	return u;
                                    }
                                }
                            }
                            """;
        
        var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
        var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));
        
        Assert.That(result, Is.Not.Null);
    }
}
