using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Models.Logging
{
    public class ArcSourceUnlocatableLog(LogLevel level, uint code, string message, string sourceFile) : ArcCompilationLogBase
    {
        public override LogLevel Level => level;

        public override uint Code => code;

        public override string FormattedMessage => $"{sourceFile}: {Message}";

        public string SourceFile => sourceFile;

        public string Message => message;
    }
}
