using System.Text;
using System.Text.Json;
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
        ArcPackageDescriptor descriptor, bool withStd = true, ILogger? logger = null)
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

        var symbols = contexts[0].ScopeTree.FlattenedNodes.OrderBy(n => n.Id).ToList();
        var functions = contexts.SelectMany((unit, pool) => unit.ScopeTree
            .GetNodes<ArcScopeTreeFunctionNodeBase>()
            .Where(f => f.GenerationResult is not null)
            .Select(f => (Function: f, Unit: unit, Pool: pool)))
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
        var functionTable = functions.Select(item =>
        {
            var function = item.Function;
            function.EntrypointPos = code.Position;
            code.Write(function.GenerationResult.GeneratedData.ToArray());
            result.SourceInformation.FunctionSources.Add(function.Id, item.Unit.UnitName);
            return new
            {
                function.Id, function.Signature, Offset = function.EntrypointPos,
                Length = function.BlockLength, Registers = function.DataCount,
                ConstantPool = item.Pool, External = function.BlockLength == 0,
                Parameters = function.Parameters.Select(p => new
                {
                    Name = p.RawFullName, Type = Type(p.DataType, symbols),
                }).ToArray(),
                ReturnType = Type(function.ReturnValueType, symbols),
                Annotations = function.Annotations.Keys.Select(a => a.Signature).ToArray()
            };
        }).ToArray();
        
        foreach (var symbol in symbols)
            result.SourceInformation.SymbolMapping.Add(symbol.Id, symbol.Signature);

        var manifest = JsonSerializer.SerializeToUtf8Bytes(new
        {
            InstructionSet = "ginkgo", FormatVersion = 1,
            Package = new { descriptor.Name, descriptor.Version, Type = descriptor.Type.ToString(), descriptor.EntrypointFunctionId },
            Symbols = symbols.Select(n => new
            {
                n.Id, n.Name, n.Signature, Kind = n.NodeType.ToString(),
                Parent = n.Parent?.Id,
                Children = n.Children.Select(c => c.Id).ToArray(),
                DataType = n is ArcScopeTreeDataTypeNode type ? new
                {
                    Kind = type.ArcDataTypeType.ToString(), Group = type.ComplexTypeGroup?.Id,
                    Enum = type.EnumType?.Id, type.IsInternal
                } : null,
                FieldType = n is ArcScopeTreeGroupFieldNode field ? Type(field.DataType, symbols) : null
            }).ToArray(),
            Functions = functionTable,
            ConstantPools = contexts.Select((unit, index) => new
            {
                Index = index, Unit = unit.UnitName,
                Values = unit.Constants.Select((c, slot) => new { Index = slot, TypeId = c.TypeId, c.Value }).ToArray()
            }).ToArray()
        });
        
        using var output = new MemoryStream();
        using var writer = new BinaryWriter(output, Encoding.UTF8, leaveOpen: true);
        writer.Write("GINKGO\0\0"u8.ToArray());
        writer.Write(1u);
        writer.Write(checked((uint)manifest.Length));
        writer.Write(checked((ulong)code.Length));
        writer.Write(manifest);
        writer.Write(code.ToArray());
        result.GeneratedData = output.ToArray();
        return result;
    }

    private static void Error(string message, ref ArcGinkgoArtifact result) =>
        result.Logs.Add(new ArcSourceUnlocatableLog(
            LogLevel.Error, 0, message, "Ginkgo artifact"));

    private static object Type(ArcDataDeclarationDescriptor declaration, List<ArcScopeTreeNodeBase>? symbols) =>
        new
        {
            Id = symbols.OfType<ArcScopeTreeDataTypeNode>()
                .FirstOrDefault(t => t.ResolvedType.TypeId == declaration.Type.TypeId)?.Id,
            declaration.Dimension, declaration.AllowNone, Mutability = declaration.Mutability.ToString()
        };
}
