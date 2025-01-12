using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.SyntaxAnalyzer
{
    public static class AntlrAdapter
    {
        public static ArcSourceCodeParser.Arc_compilation_unitContext ParseCompilationUnit(string text, ILogger logger)
        {
            logger.LogDebug("Parsing compilation unit");

            var stream = new AntlrInputStream(text);
            var lexer = new ArcSourceCodeLexer(stream, TextWriter.Null, TextWriter.Null);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ArcSourceCodeParser(tokens, TextWriter.Null, TextWriter.Null);

            return parser.arc_compilation_unit();
        }
    }
}
