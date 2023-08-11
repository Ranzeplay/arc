// See https://aka.ms/new-console-template for more information
using Arc.Compiler.Lexer;
using Arc.Compiler.Parser;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.CompilerCommandGenerator.Builders;
using Arc.CompilerCommandGenerator.Models;


var startTime = DateTime.UtcNow;

var sourceFilePath = args[0] ?? string.Empty;

if (!File.Exists(sourceFilePath))
{
    Console.WriteLine("Invalid file");
    return;
}

var text = File.ReadAllText(sourceFilePath);
var source = new SourceFile(sourceFilePath, text);
var tokens = Tokenizer.Tokenize(source, true);

Console.WriteLine($"[SUCCESS] Tokenized source file, identified {tokens.Tokens.Length} token(s)");

var parsingModel = new ExpressionBuildModel(tokens.Tokens, Array.Empty<DataDeclarator>(), Array.Empty<FunctionDeclarator>());
var functionDeclarators = Pipeline.BuildAllFunctionDeclarators(parsingModel);
parsingModel.DeclaredFunctions = functionDeclarators.ToArray();
var parsingResult = Pipeline.BuildAll(parsingModel);
if(parsingResult == null)
{
    Console.WriteLine("[FAIL] Failed to parse source file");
    return;
}

var metadata = new PackageMetadata(0, 2, 2, 2, 0, 2);
var result = new PartialGenerationResult();

foreach (var func in parsingResult.DeclaredFunctions)
{
    var context = new GenerationContext<FunctionBlock>(func, new(), new(), functionDeclarators.ToList(), new(), new(), metadata);
    var partialResult = FunctionCommandGroup.Build(context);
    result.Combine(partialResult);
}

Console.WriteLine("[SUCCESS] Command generation complete");

var reloc = new FinalRelocationContext
{
    Commands = result.Commands.ToArray(),
    PackageMetadata = metadata,
    GeneratedConstants = result.GeneratedConstants.ToArray(),
    GeneratedFunctions = functionDeclarators.ToArray(),
    GlobalData = Array.Empty<DataDeclarator>(),
    RelocationReferences = result.RelocationReferences.ToArray(),
    RelocationTargets = result.RelocationTargets.ToArray(),
};

reloc.ConvertRelocationTargets();
reloc.ApplyAllRelocation();

Console.WriteLine("[SUCCESS] Relocation complete");

var outputFilePath = args[1];

var outputContent = Enumerable.Concat(reloc.PackageMetadata.Write(), reloc.WriteConstantsTable());
outputContent = Enumerable.Concat(outputContent, reloc.WriteFunctionTable());
outputContent = Enumerable.Concat(outputContent, reloc.Commands);

File.WriteAllBytes(outputFilePath, outputContent.ToArray());

Console.WriteLine("[SUCCESS] Package written");

var endTime = DateTime.UtcNow;

var span = endTime - startTime;

Console.WriteLine($"Operation completed in {span.TotalMilliseconds}ms");
