using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.Tests.PackageGeneration
{
    [CancelAfter(1000)]
    [Category("PackageGeneration")]
    internal class FunctionStatements
    {
        [Test]
        public void EmptyFunction()
        {
            var text = "namespace Arc::Program { public func main(): val none {} }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text);
            var unit = new ArcCompilationUnit(compilationUnitContext, "test");
            var result = Flow.GenerateUnit(unit);

            Assert.That(result.Symbols, Has.Count.EqualTo(ArcPersistentData.BaseTypes.Count() + 2));
        }

        [Test]
        public void FunctionWithArguments()
        {
            var text = "namespace Arc::Program { public func main(var args: val string[]): val int {} }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text);
            var unit = new ArcCompilationUnit(compilationUnitContext, "test");
            var result = Flow.GenerateUnit(unit);

            Assert.That(result.Symbols, Has.Count.EqualTo(ArcPersistentData.BaseTypes.Count() + 2));
        }

        [Test]
        public void FunctionWithLessStatements()
        {
            var text = "namespace Arc::Program { public func main(): val int { var a: val int; const b: ref int; } }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text);
            var unit = new ArcCompilationUnit(compilationUnitContext, "test");
            var result = Flow.GenerateUnit(unit);

            Assert.That(result.Symbols, Has.Count.EqualTo(ArcPersistentData.BaseTypes.Count() + 2));
        }

        [Test]
        public void FunctionWithAssignmentExpression()
        {
            var text = @"namespace Arc::Program {
                            public func main(): val int {
                                var a: val int; a = 2; a = 2 + 3;
                            }
                        }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text);
            var unit = new ArcCompilationUnit(compilationUnitContext, "test");
            var result = Flow.GenerateUnit(unit);

            Assert.That(result.Symbols, Has.Count.EqualTo(ArcPersistentData.BaseTypes.Count() + 2));
            Assert.That(result.Constants.Count(), Is.EqualTo(3));
        }

        [Test]
        public void FunctionWithBlockStatements()
        {
            var text = @"
                        namespace Arc::Program { 
                            public func main(): val int { 
                                if (2 < 3) { var a: val int; a = 1; } 
                                else { var b: val int; b = 2; }
                                while (2 < 3) { var c: val int; c = 3; } 
                            }
                        }";
            var compilationUnitContext = AntlrAdapter.ParseCompilationUnit(text);
            var unit = new ArcCompilationUnit(compilationUnitContext, "test");
            var result = Flow.GenerateUnit(unit);

            Assert.That(result.Symbols, Has.Count.EqualTo(ArcPersistentData.BaseTypes.Count() + 2));
        }
    }
}
