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
                                public func main<T>(): int { 
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
                                public func main<T>(const t: T): int { 
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
                                public func main<T>(const t: T): T { 
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
                                public func main<T>(const t: T): int {
                                    const i: T;
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
                                    private field var item: T;
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
                                    private field var item: T;
                                    
                                    public func getFoo(var self: G): T
                                    {
                                        const i: T;
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
                                    public func getFoo<U>(var u: U, var t: T): U
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

    [Test]
    public void GenericsOnFunctionCall()
    {
        const string text = """
                            link Arc::Std::Collection;
                            link Arc::Std::Compilation;
                            
                            namespace Arc::Program {
                                @Entrypoint
                                public func main(const args: string[]): int {
                                    const list: List<string>;
                                    list = new List<string>(args);
                                    
                                    var size: int;
                                    size = list.getSize();
                                    
                                    return 0;
                                }
                            }
                            """;
        
        var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
        var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
        var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Executable));
        
        Assert.That(result, Is.Not.Null);
    }
}
