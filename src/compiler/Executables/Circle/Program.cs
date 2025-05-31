using Arc.Compiler.Circle;
using System.CommandLine;

var command = CommandBuilder.BuildCommand();
await command.InvokeAsync(args);
