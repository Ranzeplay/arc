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
                Description = "Write optional debugging information to a separate file",
                DefaultValueFactory = _ => null,
            };
            var dryRun = new Option<bool>("--dry-run")
            {
                Description = "Perform a dry run without generating output",
                DefaultValueFactory = _ => false,
            };
            var ginkgoOption = new Option<bool>("--ginkgo")
            {
                Description = "Use Ginkgo generation rules and emit a self-contained binary package",
                DefaultValueFactory = _ => false,
            };
            var compressOption = new Option<bool>("--compress")
            {
                Description = "Compress the Ginkgo package with Zstandard",
            };

            var command = new RootCommand("The compiler CLI for the Arc programming language.")
            {
                inputArgument,
                outputOption,
                packageTypeOption,
                noStdOption,
                withSourceInfoOption,
                dryRun,
                ginkgoOption,
                compressOption,
                logLevelOption,
            };
            command.TreatUnmatchedTokensAsErrors = true;

            command.Validators.Add(context =>
            {
                if (context.GetValue(inputArgument)?.Length == 0)
                {
                    context.AddError("No input file path provided");
                }
                if (context.GetValue(compressOption) && !context.GetValue(ginkgoOption))
                    context.AddError("--compress requires --ginkgo");
            });

            command.SetAction(pr =>
            {
                return CommandHandler.HandleCommand(pr.GetValue(inputArgument) ?? [], pr.GetValue(outputOption) ?? "",
                    pr.GetValue(packageTypeOption),
                    pr.GetValue(noStdOption),
                    pr.GetValue(logLevelOption), pr.GetValue(withSourceInfoOption), pr.GetValue(dryRun), pr.GetValue(ginkgoOption), pr.GetValue(compressOption));
            });
            return command;
        }
    }
}
