using Arc.Compiler.PackageGenerator.Ginkgo.Encoders;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.StdlibSource;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

public static class ArcGinkgoArtifactGenerator
{
    public static ArcGinkgoArtifact Generate(IEnumerable<ArcCompilationUnit> units,
        ArcPackageDescriptor descriptor, bool withStd = true, ILogger? logger = null, bool compress = false)
    {
        var result = new ArcGinkgoArtifact();
        var inputs = units.ToList();
        if (inputs.Count == 0)
        {
            Error("No input compilation units", ref result);
            return result;
        }
        logger ??= inputs[0].Logger;
        var standardUnits = withStd ? ArcStdlibLoader.LoadSyntax(logger).ToList() : [];
        inputs.AddRange(standardUnits);
        var (contexts, logs) = ArcGinkgoUnitGenerator.Generate(inputs, descriptor, standardUnits);
        result.Logs.AddRange(logs);
        if (result.DiscardResult) return result;

        var symbols = contexts.SelectMany(context => context.ScopeTree.FlattenedNodes)
            .GroupBy(node => node.Id).Select(group => group.First()).OrderBy(n => n.Id).ToList();
        var functions = contexts.SelectMany((unit, unitIndex) => unit.ScopeTree
            .GetNodes<ArcScopeTreeFunctionNodeBase>()
            .Where(f => f.GenerationResult is not null)
            .Select(f => (Function: f, Unit: unit, UnitIndex: unitIndex)))
            .GroupBy(f => f.Function.Id).Select(g => g.First()).OrderBy(f => f.Function.Id).ToList();
        var entrypoints = functions.Where(f => f.Function.Annotations.Keys.Any(a =>
            a.Id == 0xb11 || a.Signature is "NArc+NStd+NCompilation+AEntrypoint" or
                "NArc+NCompilation+AEntrypoint")).ToList();
        
        if (descriptor.Type == ArcPackageType.Executable)
        {
            if (entrypoints.Count != 1 || entrypoints[0].Function.BlockLength == 0)
            {
                Error("Ginkgo executable requires exactly one defined @Entrypoint function", ref result);
                return result;
            }
            descriptor.EntrypointFunctionId = entrypoints[0].Function.Id;
        }

        using var code = new MemoryStream();
        foreach (var item in functions)
        {
            var function = item.Function;
            // Each unit has its own scope copy; emit the copy whose body was generated.
            symbols[symbols.FindIndex(n => n.Id == function.Id)] = function;
            function.EntrypointPos = code.Position;
            code.Write(function.GenerationResult.GeneratedData.ToArray());
            result.SourceInformation.FunctionSources.Add(function.Id, item.Unit.UnitName);
        }
        
        foreach (var symbol in symbols)
            result.SourceInformation.SymbolMapping.Add(symbol.Id, symbol.Signature);

        try
        {
            result.GeneratedData = ArcGinkgoPackageEncoder.Encode(descriptor, symbols,
                functions.ToDictionary(f => f.Function.Id, f => checked((uint)f.UnitIndex)), contexts, code.ToArray(), compress);
        }
        catch (InvalidDataException exception)
        {
            Error(exception.Message, ref result);
        }
        return result;
    }

    private static void Error(string message, ref ArcGinkgoArtifact result) =>
        result.Logs.Add(new ArcSourceUnlocatableLog(
            LogLevel.Error, 0, message, "Ginkgo artifact"));
}
