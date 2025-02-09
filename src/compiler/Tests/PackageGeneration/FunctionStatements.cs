using Arc.Compiler.PackageGenerator;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration
{
    [CancelAfter(1000)]
    [Category("PackageGeneration")]
    internal class FunctionStatements
    {
        private readonly ILogger _logger = LoggerFactory.Create(builder => { }).CreateLogger<FunctionStatements>();

        [Test]
        public void EmptyFunction()
        {
            _logger.LogInformation("Running EmptyFunction test");
            var text = "namespace Arc::Program { public func main(): val none {} }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = Flow.GenerateUnits([unit]);

            Assert.That(result.Symbols, Has.Count.EqualTo(15));
        }

        [Test]
        public void FunctionWithArguments()
        {
            _logger.LogInformation("Running FunctionWithArguments test");
            var text = "namespace Arc::Program { public func main(var args: val string[]): val int {} }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = Flow.GenerateUnits([unit]);

            Assert.That(result.Symbols, Has.Count.EqualTo(15));
        }

        [Test]
        public void FunctionWithLessStatements()
        {
            _logger.LogInformation("Running FunctionWithLessStatements test");
            var text = "namespace Arc::Program { public func main(): val int { var a: val int; const b: ref int; } }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = Flow.GenerateUnits([unit]);

            Assert.That(result.Symbols, Has.Count.EqualTo(15));
        }

        [Test]
        public void FunctionWithAssignmentExpression()
        {
            _logger.LogInformation("Running FunctionWithAssignmentExpression test");
            var text = @"namespace Arc::Program {
                                public func main(): val int {
                                    var a: val int; a = 2; a = 2 + 3;
                                }
                            }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = Flow.GenerateUnits([unit]);

            Assert.Multiple(() =>
            {
                Assert.That(result.Symbols, Has.Count.EqualTo(15));
                Assert.That(result.Constants.Count, Is.EqualTo(3));
            });
        }

        [Test]
        public void FunctionWithBlockStatements()
        {
            _logger.LogInformation("Running FunctionWithBlockStatements test");
            var text = @"
                            namespace Arc::Program { 
                                public func main(): val int { 
                                    if (2 < 3) { var a: val int; a = 1; } 
                                    else { var b: val int; b = 2; }
                                    while (2 < 3) { var c: val int; c = 3; } 
                                }
                            }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var result = Flow.GenerateUnits([unit]);

            Assert.That(result.Symbols, Has.Count.EqualTo(15));
        }
    }
}
