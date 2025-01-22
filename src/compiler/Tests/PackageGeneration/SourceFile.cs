using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace Arc.Compiler.Tests.PackageGeneration
{
    [Category("PackageGeneration")]
    [CancelAfter(1000)]
    internal class SourceFile
    {
        private readonly ILogger _logger = LoggerFactory.Create(builder => { }).CreateLogger<SourceFile>();

        private readonly string _text = @"
			link Arc::Std;
			
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

        [Test]
        public void SingleCompilationUnit()
        {
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(_text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var context = Flow.GenerateUnit(unit);
            Assert.That(context.Symbols, Has.Count.EqualTo(ArcPersistentData.BaseTypes.Count() + 11));
        }
        [Test]
        public void DumpTest()
        {
            var compilationUnit = AntlrAdapter.ParseCompilationUnit(_text, _logger);
            var syntaxUnit = new ArcCompilationUnit(compilationUnit, _logger, "test");
            var context = Flow.GenerateUnit(syntaxUnit);

            context.PackageDescriptor = new ArcPackageDescriptor()
            {
                Type = ArcPackageType.Executable,
                Name = "Test",
                Version = 0,
                RootGroupTableEntryPos = 0,
                RootFunctionTableEntryPos = 0,
                RootConstantTableEntryPos = 0,
                RegionTableEntryPos = 0,
                EntrypointFunctionId = 0,
                DataAlignmentLength = 8
            };


            var outputStream = Flow.DumpFullByteStream(context);

            Assert.That(outputStream, Is.Not.Null);
        }
    }
}
