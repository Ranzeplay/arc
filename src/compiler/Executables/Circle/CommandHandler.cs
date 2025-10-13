using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Text.Json;

namespace Arc.Compiler.Circle
{
    internal static class CommandHandler
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static void HandleCommand(string[] inputs, string output, ArcPackageType packageType, bool noStd, LogLevel logLevel, string? sourceInfoPath)
        {
            var logger = LoggerFactory.Create(builder =>
            {
                builder.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.IncludeScopes = false;
                    options.ColorBehavior = LoggerColorBehavior.Default;
                    options.UseUtcTimestamp = false;
                });
                builder.SetMinimumLevel(logLevel);
            }).CreateLogger("Circle CLI");

            logger.LogInformation("Processing {} file(s)", inputs.Length);

            // We can ensure that inputs is not empty because of the validator
            logger.LogInformation("Analyzing source code structure");
            var compilationUnits = inputs
                .Select(t => new { Content = File.ReadAllText(t), Path = t })
                .Select(t => new { Context = AntlrAdapter.ParseCompilationUnit(t.Content, logger), t.Path })
                .Select(t => new ArcCompilationUnit(t.Context, logger, t.Path));

            logger.LogInformation("Encoding instructions");
            var context = ArcCombinedUnitGenerator.GenerateUnits(compilationUnits, ArcPackageDescriptor.Default(packageType), !noStd);

            context.SetEntrypointFunctionId();

            foreach (var log in context.Logs)
            {
                logger.Log(log.Level, "{}", log.FormattedMessage);
            }

            if (context.Logs.Count != 0)
            {
                logger.LogInformation("Compilation generated with {0} info(s), {1} warning(s), {2} error(s)",
                    context.Logs.Count(l => l.Level == LogLevel.Information),
                    context.Logs.Count(l => l.Level == LogLevel.Warning),
                    context.Logs.Count(l => l.Level == LogLevel.Error)
                );
            }

            if (context.Logs.Any(l => l.Level == LogLevel.Error))
            {
                logger.LogError("Failed to generate package due to {} error(s) above", context.Logs.Count(e => e.Level >= LogLevel.Error));
                return;
            }

            logger.LogInformation("Generating byte stream");

            var outputStream = context.DumpFullByteStream();


            File.WriteAllBytes(output, [.. outputStream]);

            if (sourceInfoPath != null)
            {
                logger.LogInformation("Generating source information");
                context.UpdateSourceSymbolInformation();

                var sourceInfoJsonText = JsonSerializer.Serialize(context.SourceInformation, options: _jsonSerializerOptions);

                File.WriteAllText(sourceInfoPath, sourceInfoJsonText);
            }

            logger.LogInformation("Done for {}", packageType.ToString().ToLowerInvariant());
        }
    }
}
