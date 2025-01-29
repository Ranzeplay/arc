using Microsoft.Extensions.Logging;
using System.CommandLine;

namespace Arc.Compiler.Circle
{
    internal static class CommandBuilder
    {
        public static RootCommand BuildCommand()
        {
            var inputOption = new Argument<string[]>("input", "The input file path");
            var outputOption = new Option<string>(["--output", "-o"], () => "./build.pkg.arc", "The output file path");
            var logLevelOption = new Option<LogLevel>(["--log-level", "-l"], () => LogLevel.Information, "The log level");

            var command = new RootCommand("Circle CLI")
            {
                inputOption,
                outputOption,
                logLevelOption
            };

            command.SetHandler(CommandHandler.HandleCommand, inputOption, outputOption, logLevelOption);
            return command;
        }
    }
}
