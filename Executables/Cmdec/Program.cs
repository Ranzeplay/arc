using Arc.Cmdec;
using CommandLine;
using Terminal.Gui;

Application.Init();

var options = Parser.Default.ParseArguments<Options>(args).Value;

var packageContent = File.ReadAllBytes(options.PackagePath!);

var package = Decoder.Decode(packageContent);
Memory.Package = package;

Application.Run<CommandListWindow>();

Application.Shutdown();
