using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration
{
    [Category("PackageGeneration")]
    [CancelAfter(1000)]
    internal class SingleCompilationUnit
    {
        private readonly ILogger _logger = LoggerFactory.Create(builder => { }).CreateLogger<SingleCompilationUnit>();

        private readonly string _text = @"
			link Arc::Std;
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

        [Test]
        public void Generation()
        {
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(_text, _logger);
            var unit = new ArcCompilationUnit(compilationUnitContext, _logger, "test");
            var context = ArcCombinedUnitGenerator.GenerateUnits([unit]);
            Assert.That(context.GlobalScopeTree.FlattenedNodes.Count(s => s.Id > 0xfff), Is.EqualTo(16));
        }

        [Test]
        public void DumpTest()
        {
            var compilationUnit = AntlrAdapter.ParseCompilationUnit(_text, _logger);
            var syntaxUnit = new ArcCompilationUnit(compilationUnit, _logger, "test");
            var context = ArcCombinedUnitGenerator.GenerateUnits([syntaxUnit]);

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

            var outputStream = context.DumpFullByteStream();

            Assert.That(outputStream, Is.Not.Null);
        }

        [Test]
        public void HelloWorld()
        {
            var text = @"
                link Arc::Std::Console;
                link Arc::Std::Compilation;
                
                namespace Program
                {
                	@Entrypoint
                	public func main(var args: val string[]): val int
                	{
                		call PrintString(""Hello, world!\n"");
                
                		return 0;
                	}	
                }
            ";

            var compilationUnit = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var syntaxUnit = new ArcCompilationUnit(compilationUnit, _logger, "test");
            var context = ArcCombinedUnitGenerator.GenerateUnits([syntaxUnit]);
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
            context.SetEntrypointFunctionId();
            var outputStream = context.DumpFullByteStream();
            Assert.That(outputStream, Is.Not.Null);
        }

        [Test]
        public void Fibonacci()
        {
            var text = @"
                link Arc::Std::Console;
                link Arc::Std::Compilation;

                namespace Arc::Program {
                	# @Export
                	@Entrypoint
                	public func main(var args: val string[]): val int {
                		var i: val int;
                		i = 0;
                		while (i < 40)
                		{
                			const value: val int;
                			value = fib(i);
                			call PrintInteger(value);
                			call PrintString(""\n"");
                			i = i + 1;
                		}
                
                		return 0;
                	}
                
                	public func fib(const n: val int): val int
                	{
                		if (n <= 1)
                		{
                			return 1;
                		}
                		else
                		{
                			return fib(n - 1) + fib(n - 2);
                		}
                	}
                }
            ";

            var compilationUnit = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var syntaxUnit = new ArcCompilationUnit(compilationUnit, _logger, "test");
            var context = ArcCombinedUnitGenerator.GenerateUnits([syntaxUnit]);
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
            context.SetEntrypointFunctionId();
            var outputStream = context.DumpFullByteStream();
            Assert.That(outputStream, Is.Not.Null);
        }
    }
}
