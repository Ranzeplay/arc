using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Text.Json;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

namespace Arc.Compiler.Circle
{
    internal static class CommandHandler
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static int HandleCommand(string[] inputs, string output, ArcPackageType packageType, bool noStd, LogLevel logLevel, string? sourceInfoPath, bool dryRun, bool ginkgo = false)
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
            if (ginkgo)
            {
                var artifact = ArcGinkgoArtifactGenerator.Generate(compilationUnits,
                    ArcPackageDescriptor.Default(packageType), !noStd, logger);
                foreach (var log in artifact.Logs)
                    logger.Log(log.Level, "{}", log.FormattedMessage);
                if (artifact.DiscardResult)
                {
                    logger.LogError("Failed to generate Ginkgo artifact due to {} error(s)",
                        artifact.Logs.Count(l => l.Level >= LogLevel.Error));
                    return 1;
                }
                if (!dryRun)
                {
                    File.WriteAllBytes(output, artifact.GeneratedData);
                    if (sourceInfoPath is not null)
                        File.WriteAllText(sourceInfoPath, JsonSerializer.Serialize(artifact.SourceInformation,
                            options: _jsonSerializerOptions));
                }
                else logger.LogInformation("dry-run specified, skipping output generation");
                logger.LogInformation("Generated draft Ginkgo artifact ({} bytes); legacy runtime execution is not supported",
                    artifact.GeneratedData.Length);
                return 0;
            }
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

            if (context.Logs.Any(l => l.Level >= LogLevel.Error))
            {
                logger.LogError("Failed to generate package due to {} error(s) above", context.Logs.Count(e => e.Level >= LogLevel.Error));
                return 1;
            }

            logger.LogInformation("Generating byte stream");

            var outputStream = context.DumpFullByteStream();

            if (!dryRun)
            {
                File.WriteAllBytes(output, [.. outputStream]);

                if (sourceInfoPath != null)
                {
                    logger.LogInformation("Generating source information");
                    context.UpdateSourceSymbolInformation();

                    var sourceInfoJsonText = JsonSerializer.Serialize(context.SourceInformation, options: _jsonSerializerOptions);

                    File.WriteAllText(sourceInfoPath, sourceInfoJsonText);
                }
            }
            else
            {
                logger.LogInformation("dry-run specified, skipping output generation");
            }

            logger.LogInformation("Done for {}", packageType.ToString().ToLowerInvariant());
            return 0;
        }
    }
}
