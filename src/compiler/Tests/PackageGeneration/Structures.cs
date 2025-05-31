using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.StdlibSource;
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

            var structure = ArcLayeredScopeTreeGenerator.GenerateUnitStructure([unit], ArcPackageDescriptor.Default(ArcPackageType.Library)).Item1.First();

            Assert.That(structure.ScopeTree.FlattenedNodes.Count(s => s.Id > 0xfff), Is.EqualTo(7));
        }

        [Test]
        public void SingleCompilationUnitWithGroup()
        {
            var text = @"
                        link Arc::Std::Compilation;
                        namespace Arc::Program {
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

            var structure = ArcLayeredScopeTreeGenerator
                .GenerateUnitStructure([unit, .. ArcStdlibLoader.LoadSyntax(_logger)], ArcPackageDescriptor.Default(ArcPackageType.Library))
                .Item1
                .First();
            Assert.That(structure.ScopeTree.FlattenedNodes.Count(s => s.Id > 0xfff), Is.EqualTo(11));
        }
    }
}
