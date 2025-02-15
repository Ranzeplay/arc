using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Arc.Compiler.Circle
{
    internal class CommandHandler
    {
        public static void HandleCommand(string[] inputs, string output, LogLevel logLevel)
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
            var context = ArcCombinedUnitGenerator.GenerateUnits(compilationUnits);
            context.PackageDescriptor = new ArcPackageDescriptor()
            {
                Type = ArcPackageType.Executable,
                Name = "Test",
                Version = 0,
                RootGroupTableEntryPos = 0,
                RootFunctionTableEntryPos = 0,
                RootConstantTableEntryPos = 0,
                RegionTableEntryPos = 0,
                EntrypointFunctionId = 0,
                DataAlignmentLength = 8
            };
            context.SetEntrypointFunctionId();

            logger.LogInformation("Generating byte stream");
            var outputStream = context.DumpFullByteStream();

            File.WriteAllBytes(output, [.. outputStream]);
        }
    }
}
