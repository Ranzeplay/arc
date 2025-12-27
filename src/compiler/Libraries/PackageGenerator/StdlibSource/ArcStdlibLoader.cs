using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Arc.Compiler.PackageGenerator.StdlibSource
{
    class ArcStdlibLoader
    {
        public static IEnumerable<ArcCompilationUnit> LoadSyntax(ILogger logger)
        {
            var compilationNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.NamespaceCompilation);
            var compilerNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit(compilationNamespaceSource, logger);
            var compilerNamespaceUnit = new ArcCompilationUnit(compilerNamespaceUnitContext, logger, "Arc::Std::Compilation");

            var arrayNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.NamespaceArray);
            var arrayNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit(arrayNamespaceSource, logger);
            var arrayNamespaceUnit = new ArcCompilationUnit(arrayNamespaceUnitContext, logger, "Arc::Std::Array");

            var consoleNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.NamespaceConsole);
            var consoleNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit(consoleNamespaceSource, logger);
            var consoleNamespaceUnit = new ArcCompilationUnit(consoleNamespaceUnitContext, logger, "Arc::Std::Console");

            var mathNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.NamespaceMath);
            var mathNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit(mathNamespaceSource, logger);
            var mathNamespaceUnit = new ArcCompilationUnit(mathNamespaceUnitContext, logger, "Arc::Std::Math");

            var collectionListNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.NamespaceCollectionList);
            var collectionListNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit(collectionListNamespaceSource, logger);
            var collectionListNamespaceUnit = new ArcCompilationUnit(collectionListNamespaceUnitContext, logger, "Arc::Std::Collection");
            
            var collectionLinkedListNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.NamespaceCollectionLinkedList);
            var collectionLinkedListNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit(collectionLinkedListNamespaceSource, logger);
            var collectionLinkedListNamespaceUnit = new ArcCompilationUnit(collectionLinkedListNamespaceUnitContext, logger, "Arc::Std::Collection");

            // var structure = ArcLayeredScopeTreeGenerator.GenerateUnitStructure([compilerNamespaceUnit, arrayNamespaceUnit, consoleNamespaceUnit]);

            return [compilerNamespaceUnit, arrayNamespaceUnit, consoleNamespaceUnit, mathNamespaceUnit, collectionListNamespaceUnit, collectionLinkedListNamespaceUnit];
        }
    }
}
