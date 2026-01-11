using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Microsoft.Extensions.Logging;
using System.CommandLine;

namespace Arc.Compiler.Circle
{
    internal static class CommandBuilder
    {
        public static RootCommand BuildCommand()
        {
            var inputArgument = new Argument<string[]>("input")
            {
                Description = "The input file path",
                Arity = ArgumentArity.ZeroOrMore,
            };
            var outputOption = new Option<string>("--output", "-o")
            {
                Description = "The output file path",
                DefaultValueFactory = _ => "./build.pkg.arc",
            };
            var logLevelOption = new Option<LogLevel>("--log-level", "-l")
            {
                Description = "The log level",
                DefaultValueFactory = _ => LogLevel.Information,
            };
            var packageTypeOption = new Option<ArcPackageType>("--package-type", "-t")
            {
                Description = "The package type",
                DefaultValueFactory = _ => ArcPackageType.Executable,
            };
            var noStdOption = new Option<bool>("--no-std")
            {
                Description = "Do not include the standard library",
                DefaultValueFactory = _ => false,
            };
            var withSourceInfoOption = new Option<string?>("--source-info")
            {
                Description = "Include source information in the package and specify path",
                DefaultValueFactory = _ => null,
            };
            var dryRun = new Option<bool>("--dry-run")
            {
                Description = "Perform a dry run without generating output",
                DefaultValueFactory = _ => false,
            };

            var command = new RootCommand("The compiler CLI for the Arc programming language.")
            {
                inputArgument,
                outputOption,
                packageTypeOption,
                noStdOption,
                withSourceInfoOption,
                dryRun,
            };
            command.TreatUnmatchedTokensAsErrors = true;

            command.Validators.Add(context =>
            {
                if (context.GetValue(inputArgument)?.Length == 0)
                {
                    context.AddError("No input file path provided");
                }
            });

            command.SetAction(pr =>
            {
                CommandHandler.HandleCommand(pr.GetValue(inputArgument) ?? [], pr.GetValue(outputOption) ?? "",
                    pr.GetValue(packageTypeOption),
                    pr.GetValue(noStdOption),
                    pr.GetValue(logLevelOption), pr.GetValue(withSourceInfoOption), pr.GetValue(dryRun));
            });
            return command;
        }
    }
}