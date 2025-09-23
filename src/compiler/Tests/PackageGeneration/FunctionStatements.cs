using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration
{
    [CancelAfter(1000)]
    [Category("PackageGeneration")]
    internal class FunctionStatements
    {
        private readonly ILogger _logger = LoggerFactory.Create(_ => { }).CreateLogger<FunctionStatements>();

        [Test]
        public void EmptyFunction()
        {
            _logger.LogInformation("Running EmptyFunction test");
            const string text = "namespace Arc::Program { public func main(): none {} }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));

            Assert.That(result.GlobalScopeTree.FlattenedNodes.Count(s => s.Id > 0xfff), Is.EqualTo(34));
        }

        [Test]
        public void FunctionWithArguments()
        {
            _logger.LogInformation("Running FunctionWithArguments test");
            const string text = "namespace Arc::Program { public func main(var args: string[]): int {} }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));

            Assert.That(result.GlobalScopeTree.FlattenedNodes.Count(s => s.Id > 0xfff), Is.EqualTo(34));
        }

        [Test]
        public void FunctionWithLessStatements()
        {
            _logger.LogInformation("Running FunctionWithLessStatements test");
            const string text = "namespace Arc::Program { public func main(): int { var a: int; const b: int; } }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));

            Assert.That(result.GlobalScopeTree.FlattenedNodes.Count(s => s.Id > 0xfff), Is.EqualTo(34));
        }

        [Test]
        public void FunctionWithAssignmentExpression()
        {
            _logger.LogInformation("Running FunctionWithAssignmentExpression test");
            const string text = """
                                namespace Arc::Program {
                                    public func main(): int {
                                        var a: int; a = 2; a = 2 + 3;
                                    }
                                }
                                """;
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));

            Assert.Multiple(() =>
            {
                Assert.That(result.GlobalScopeTree.FlattenedNodes.Count(s => s.Id > 0xfff), Is.EqualTo(34));
                Assert.That(result.Constants, Has.Count.EqualTo(37));
            });
        }

        [Test]
        public void FunctionWithBlockStatements()
        {
            _logger.LogInformation("Running FunctionWithBlockStatements test");
            const string text = """
                                namespace Arc::Program { 
                                    public func main(): int { 
                                        if (2 < 3) { var a: int; a = 1; } 
                                        else { var b: int = 2; }
                                        while (2 < 3) { var c: int; c = 3; } 
                                    }
                                }
                                """;
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));

            Assert.That(result.GlobalScopeTree.FlattenedNodes.Count(s => s.Id > 0xfff), Is.EqualTo(34));
        }
        
        [Test]
        public void FunctionWithIntegratedDeclarationAssignmentStatements()
        {
            _logger.LogInformation("Running FunctionWithBlockStatements test");
            const string text = """
                                namespace Arc::Program { 
                                    public func main(): none { 
                                        var a: int = 1;
                                    }
                                }
                                """;
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = ArcCombinedUnitGenerator.GenerateUnits([unit], ArcPackageDescriptor.Default(ArcPackageType.Library));

            Assert.That(result, Is.Not.Null);
        }
    }
}
