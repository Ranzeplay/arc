using Arc.Compiler.Circle;
using System.CommandLine;

var command = CommandBuilder.BuildCommand();
return await command.Parse(args).InvokeAsync();
