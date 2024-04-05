using Antlr4.Runtime;

namespace Arc.Compiler.SyntaxAnalyzer
{
    public class ANTLRAdapter
    {
        public static ArcSourceCodeParser.Arc_compilation_unitContext ParseCompilationUnit(string text)
        {
            var stream = new AntlrInputStream(text);
            var lexer = new ArcSourceCodeLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ArcSourceCodeParser(tokens);

            return parser.arc_compilation_unit();
        }
    }
}
