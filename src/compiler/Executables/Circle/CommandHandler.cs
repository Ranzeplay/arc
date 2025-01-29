using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.Circle
{
    internal class CommandHandler
    {
        public static void HandleCommand(string[] inputs, string output, LogLevel logLevel)
        {
            var logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(logLevel);
            }).CreateLogger("Circle CLI");

            if (inputs.Length == 0)
            {
                logger.LogError("No input file path provided");
                return;
            }

            var compilationUnits = inputs
                .Select(t => new { Content = File.ReadAllText(t), Path = t })
                .Select(t => new { Context = AntlrAdapter.ParseCompilationUnit(t.Content, logger), t.Path })
                .Select(t => new ArcCompilationUnit(t.Context, logger, t.Path));

            // TODO: Implement linking and generation
        }
    }
}
