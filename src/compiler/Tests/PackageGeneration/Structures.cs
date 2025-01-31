using Arc.Compiler.PackageGenerator;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration
{
    [CancelAfter(1000)]
    [Category("PackageGeneration")]
    internal class Structures
    {
        private readonly ILogger _logger = LoggerFactory.Create(builder => { }).CreateLogger<Structures>();

        [Test]
        public void SingleCompilationUnitWithFunctions()
        {
            var text = @"namespace Arc::Program {
                            public func main(var args: val string[]): val int {
                                var a: val int; a = 2; a = 2 + 3; return 0;
                            }
                            public func foo(): val int { return -1; }
                            public func bar(const lazyFox: ref string): ref bool {
                                return true;
                            }
                        }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");

            var structure = Flow.GenerateUnitStructure(unit);

            Assert.That(structure.Symbols.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SingleCompilationUnitWithGroup()
        {
            var text = @"namespace Arc::Program {
                            @Export
                        	public group ArcExample {
                        		@Getter
                        		public field const foo: val string;
                        		@Accessor
                        		public field var bar: val string;
                        
                        		private func eval(): val bool {
                        			return false;
                        		}
                        
                        		public func empty(): val none {}
                        	}
                        }";

            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");

            var structure = Flow.GenerateUnitStructure(unit);
            Assert.That(structure.Symbols, Has.Count.EqualTo(2));
        }
    }
}
