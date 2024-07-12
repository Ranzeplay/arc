using CommandLine;

namespace Arc.Cmdec
{
    internal class Options
    {
        [Option('v', "verbose", Default = false, HelpText = "Verbose output.")]
        public bool Verbose { get; set; }

        [Option('p', "package", HelpText = "The file path of the package", Required = true)]
        public string? PackagePath { get; set; }
    }
}
