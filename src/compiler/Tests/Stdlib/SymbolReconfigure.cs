using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Arc.Compiler.Tests.Stdlib
{
    [Category("Stdlib")]
    [CancelAfter(1000)]
    class SymbolReconfigure
    {
        private readonly ILogger _logger = LoggerFactory.Create(builder => { }).CreateLogger<SymbolReconfigure>();

        [Test]
        public void SymbolId()
        {
            var compilationNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.NamespaceCompilation);
            var compilerNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit(compilationNamespaceSource, _logger);
            var compilerNamespaceUnit = new ArcCompilationUnit(compilerNamespaceUnitContext, _logger, "Arc::Std::Compilation");

            var arrayNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.NamespaceArray);
            var arrayNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit(arrayNamespaceSource, _logger);
            var arrayNamespaceUnit = new ArcCompilationUnit(arrayNamespaceUnitContext, _logger, "Arc::Std::Array");

            var consoleNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.NamespaceConsole);
            var consoleNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit(consoleNamespaceSource, _logger);
            var consoleNamespaceUnit = new ArcCompilationUnit(consoleNamespaceUnitContext, _logger, "Arc::Std::Console");

            var context = ArcCombinedUnitGenerator.GenerateUnits([compilerNamespaceUnit, arrayNamespaceUnit, consoleNamespaceUnit], false);

            Assert.That(context.GlobalScopeTree.FlattenedNodes.Any(x => x.Id == 0xa1));
        }
    }
}
