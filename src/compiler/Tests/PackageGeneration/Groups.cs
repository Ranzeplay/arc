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
        private readonly ILogger _logger = LoggerFactory.Create(builder => { }).CreateLogger<Groups>();

        private readonly string _text = @"			
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
            ";

        [Test]
        public void TestAssignment()
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
            context.SetEntrypointFunctionId();
            var outputStream = context.DumpFullByteStream();
            Assert.That(outputStream, Is.Not.Null);
        }
    }
}
