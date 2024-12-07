using Arc.Compiler.PackageGenerator;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.SyntaxAnalyzer;
using Arc.Compiler.SyntaxAnalyzer.Models;

Console.WriteLine("Arc compiler CLI");

var text = File.ReadAllText(args[0]);
var compilationUnit = AntlrAdapter.ParseCompilationUnit(text);
var syntaxUnit = new ArcCompilationUnit(compilationUnit, args[0]);
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
