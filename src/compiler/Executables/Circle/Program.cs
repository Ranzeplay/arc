using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;


var logLevelArg = args[2] ?? "";
var logLevel = logLevelArg switch
{
    "trace" => LogLevel.Trace,
    "debug" => LogLevel.Debug,
    "info" => LogLevel.Information,
    "warn" => LogLevel.Warning,
    "error" => LogLevel.Error,
    "critical" => LogLevel.Critical,
    _ => LogLevel.Information
};

var logger = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(logLevel);
}).CreateLogger("Circle CLI");

logger.LogInformation("Circle CLI");

var text = File.ReadAllText(args[0]);
var compilationUnit = AntlrAdapter.ParseCompilationUnit(text, logger);
var syntaxUnit = new ArcCompilationUnit(compilationUnit, logger, args[0]);
var context = Flow.GenerateUnit(syntaxUnit);

context.PackageDescriptor = new ArcPackageDescriptor()
{
    Type = ArcPackageType.Executable,
    Name = "Test",
    Version = 255,
    RootGroupTableEntryPos = 0,
    RootFunctionTableEntryPos = 0,
    RootConstantTableEntryPos = 0,
    RegionTableEntryPos = 0xffffffff,
    EntrypointFunctionId = 0,
    DataAlignmentLength = 8
};

var outputStream = Flow.DumpFullByteStream(context);

File.WriteAllBytes(args[1], outputStream.ToArray());
