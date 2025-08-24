using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration
{
    [Category("PackageGeneration")]
    [CancelAfter(1000)]
    internal class MultipleCompilationUnits
    {
        private readonly ILogger _logger = LoggerFactory.Create(_ => { }).CreateLogger<SingleCompilationUnit>();

        private readonly string _text1 = @"
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
			
				@Export
				@Entrypoint
				public func main(var args: val string[]): val none {
					var a: val int;
					# a = [Console].readLn();
					var b: val int;
					a = 114514;
					b = 1919810;
					const c: val int;
					if (a > b) { c = a + b; }
					elif (a < b) { c = a * b; }
					else { c = a - b; }
			
					# call [Console].PrintLn(""Hello, world!"");
                    
                    call test();

                    while (1 > 0) { a = 2233; }
			
					return [Arc::Program].test();
				}
                
                public func test(): val int {
        	        return 0;
    	        }
			}
            ";

        private readonly string _text2 = @"
			namespace Program::Test {
				public func test(): val int {
			        	return 0;
			    	}
			}
            ";

        [Test]
        public void Generation()
        {
            var compilationUnitContext1 = AntlrAdapter.ParseCompilationUnit(_text1, _logger);
            var compilationUnitContext2 = AntlrAdapter.ParseCompilationUnit(_text2, _logger);
            var unit1 = new ArcCompilationUnit(compilationUnitContext1, _logger, "test1");
            var unit2 = new ArcCompilationUnit(compilationUnitContext2, _logger, "test2");

            var context = ArcCombinedUnitGenerator.GenerateUnits([unit1, unit2], ArcPackageDescriptor.Default(ArcPackageType.Library));

            var outputStream = context.DumpFullByteStream();

            Assert.That(context, Is.Not.Null);
            Assert.That(outputStream, Is.Not.Null);
        }
    }
}
