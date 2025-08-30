using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Tests.PackageGeneration
{
    [Category("PackageGeneration")]
    [CancelAfter(1000)]
    internal class Groups
    {
        private readonly ILogger _logger = LoggerFactory.Create(_ => { }).CreateLogger<Groups>();

        [Test]
        public void TestAssignment()
        {
            const string text = """
                                link Arc::Std::Compilation;
                                namespace Arc::Program {
                                	public group Foo {
                                		@Getter
                                		public field var fitem: val int;
                                	}

                                	public group Bar {
                                		@Getter
                                		public field var bitem: val int;

                                		public func getFoo(var self: ref Bar, var addend: val int): val Foo
                                		{
                                			var foo: val Foo;
                                			foo.fitem = self.bitem + addend;

                                			return foo;
                                		}
                                	}

                                	@Export
                                	@Entrypoint
                                	public func main(var args: val string[]): val int {
                                		var a: val Bar;
                                		a.bitem = 42;

                                		var b: val Foo;
                                		b = a.getFoo(37413);

                                		return 0;
                                	}
                                }
                                """;
            
            var compilationUnit = AntlrAdapter.ParseCompilationUnit(text, _logger);
            var syntaxUnit = new ArcCompilationUnit(compilationUnit, _logger, "test");
            var context = ArcCombinedUnitGenerator.GenerateUnits([syntaxUnit], ArcPackageDescriptor.Default(ArcPackageType.Library));
            context.SetEntrypointFunctionId();
            var outputStream = context.DumpFullByteStream();
            Assert.That(outputStream, Is.Not.Null);
        }

        [Test]
        public void TestLifecycleFunctions()
        {
	        const string text = """
	                            link Arc::Std::Compilation;
	                            namespace Arc::Program {
	                            	public group Foo {
	                            		public constructor(): val Foo {}
	                            	}

	                            	@Export
	                            	@Entrypoint
	                            	public func main(var args: val string[]): val int {
	                            		var b: val Foo;
	                            		b = new Foo();

	                            		return 0;
	                            	}
	                            }
	                            """;
            
	        var compilationUnit = AntlrAdapter.ParseCompilationUnit(text, _logger);
	        var syntaxUnit = new ArcCompilationUnit(compilationUnit, _logger, "test");
	        var context = ArcCombinedUnitGenerator.GenerateUnits([syntaxUnit], ArcPackageDescriptor.Default(ArcPackageType.Library));
	        context.SetEntrypointFunctionId();
	        var outputStream = context.DumpFullByteStream();
	        Assert.That(outputStream, Is.Not.Null);
        }
    }
}
