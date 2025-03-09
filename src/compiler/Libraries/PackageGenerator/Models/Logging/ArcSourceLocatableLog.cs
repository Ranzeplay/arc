using Antlr4.Runtime;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Models.Logging
{
    public class ArcSourceLocatableLog(LogLevel level, uint code, string message, string sourceFile, ParserRuleContext context) : ArcCompilationLogBase
    {
        public override LogLevel Level => level;

        public override uint Code => code;

        public override string FormattedMessage => $"{sourceFile} [({Begin})~({End})]: {Message}";

        public string SourceFile => sourceFile;

        private Position Begin { get; } = new Position(context.Start);

        private Position End { get; } = new Position(context.Stop);

        public string Message => message;

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
