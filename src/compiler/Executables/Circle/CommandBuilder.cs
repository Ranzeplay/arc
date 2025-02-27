using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Microsoft.Extensions.Logging;
using System.CommandLine;

namespace Arc.Compiler.Circle
{
    internal static class CommandBuilder
    {
        public static RootCommand BuildCommand()
        {
            var inputArgument = new Argument<string[]>("input", "The input file path");
            var outputOption = new Option<string>(["--output", "-o"], () => "./build.pkg.arc", "The output file path");
            var logLevelOption = new Option<LogLevel>(["--log-level", "-l"], () => LogLevel.Information, "The log level");
            var packageTypeOption = new Option<ArcPackageType>(["--package-type", "-t"], () => ArcPackageType.Executable, "The package type");
            var noStdOption = new Option<bool>(["--no-std"], "Do not include the standard library");

            var command = new RootCommand()
            {
                inputArgument,
                outputOption,
                packageTypeOption,
                noStdOption,
            };

            command.Name = "Circle";
            command.Description = "The compiler CLI for the Arc programming language.";
            command.TreatUnmatchedTokensAsErrors = true;

            command.AddGlobalOption(logLevelOption);
            command.AddValidator(context =>
            {
                if (context.GetValueForArgument(inputArgument).Length == 0)
                {
                    context.ErrorMessage = "No input file path provided";
                }
            });

            command.SetHandler(CommandHandler.HandleCommand, inputArgument, outputOption, packageTypeOption, noStdOption, logLevelOption);
            return command;
        }
    }
}
