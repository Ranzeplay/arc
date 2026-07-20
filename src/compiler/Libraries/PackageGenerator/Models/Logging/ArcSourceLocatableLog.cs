using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Models.Logging
{
    public class ArcSourceLocatableLog : ArcCompilationLogBase
    {
        private readonly LogLevel _level;
        private readonly uint _code;
        private readonly string _message;
        private readonly string _sourceFile;

        public ArcSourceLocatableLog(LogLevel level, uint code, string message, string sourceFile, ParserRuleContext context)
        {
            _level = level;
            _code = code;
            _message = message;
            _sourceFile = sourceFile;
            Begin = new Position(context.Start);
            End = new Position(context.Stop);
        }
        
        public ArcSourceLocatableLog(LogLevel level, uint code, string message, string sourceFile, IArcTraceable<ParserRuleContext> locatable)
        {
            var context = locatable.Context;
            
            _level = level;
            _code = code;
            _message = message;
            _sourceFile = sourceFile;
            Begin = new Position(context.Start);
            End = new Position(context.Stop);
        }

        public override LogLevel Level => _level;

        public override uint Code => _code;

        public override string FormattedMessage => $"{_sourceFile} [({Begin})~({End})]: {Message}";

        public string SourceFile => _sourceFile;

        private Position Begin { get; }

        private Position End { get; }

        public string Message => _message;

        private readonly struct Position(IToken token)
        {
            public int Line { get; } = token.Line;
            public int Column { get; } = token.Column;

            public override string ToString()
            {
                return $"{Line},{Column}";
            }
        }
    }
}
