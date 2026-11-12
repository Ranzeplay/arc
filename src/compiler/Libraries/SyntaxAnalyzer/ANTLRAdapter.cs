using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.SyntaxAnalyzer
{
    public static class AntlrAdapter
    {
        public static ArcSourceCodeParser.Arc_compilation_unitContext ParseCompilationUnit(string text, ILogger logger,
            string sourceName = "<input>")
        {
            logger.LogDebug("Parsing compilation unit");

            var stream = new AntlrInputStream(text);
            var errors = new SyntaxErrorListener(sourceName);
            var lexer = new ArcSourceCodeLexer(stream);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(errors);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ArcSourceCodeParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errors);

            var context = parser.arc_compilation_unit();
            if (errors.Diagnostics.Count > 0)
                throw new ArcSyntaxException(errors.Diagnostics);
            return context;
        }

        private sealed class SyntaxErrorListener(string sourceName) : BaseErrorListener, IAntlrErrorListener<int>
        {
            public List<string> Diagnostics { get; } = [];

            public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol,
                int line, int charPositionInLine, string msg, RecognitionException e)
                => Add(line, charPositionInLine, msg);

            public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol,
                int line, int charPositionInLine, string msg, RecognitionException e)
                => Add(line, charPositionInLine, msg);

            private void Add(int line, int column, string message)
                => Diagnostics.Add($"{sourceName} ({line},{column + 1}): {message}");
        }
    }
}
