using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Models.Logging
{
    public abstract class ArcCompilationLogBase
    {
        public abstract LogLevel Level { get; }

        public abstract uint Code { get; }

        public abstract string FormattedMessage { get; }
    }
}
