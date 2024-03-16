using Antlr4.Runtime;
using Arc.Compiler.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer
{
    public class AntlrParserAdapter
    {
        public static ArcSourceCodeParser.Compilation_unitContext ParseCompilationUnit(string text)
        {
            var stream = new AntlrInputStream(text);
            var lexer = new ArcSourceCodeLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ArcSourceCodeParser(tokens);

            return parser.compilation_unit();
        }
    }
}
