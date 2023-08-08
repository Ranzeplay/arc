using System.CommandLine;

namespace Arc.Compiler.Circle
{
    internal class CommandManager
    {
        public static void ParseAndRun(string[] args)
        {
            var cmdecOption = new Option<string>(name: "cmdec", description: "Use Arc.Cmdec to inspect a package");

            var root = new RootCommand("Arc.Compiler.Circle CLI Tool");
            root.AddOption(cmdecOption);

            root.Invoke(args);
        }
    }
}
