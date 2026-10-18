using Arc.Compiler.PackageGenerator.Ginkgo.Generator;
using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Legacy = Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

/// <summary>Generates Ginkgo function bodies from an already resolved compilation unit.</summary>
public static class ArcGinkgoUnitGenerator
{
    public static (IReadOnlyList<ArcGinkgoUnitGenerationContext> Units, IReadOnlyList<ArcCompilationLogBase> Logs)
        Generate(IEnumerable<Arc.Compiler.SyntaxAnalyzer.Models.ArcCompilationUnit> units,
            Arc.Compiler.PackageGenerator.Models.Descriptors.ArcPackageDescriptor descriptor,
            IReadOnlyCollection<Arc.Compiler.SyntaxAnalyzer.Models.ArcCompilationUnit>? deferredUnits = null)
    {
        var inputs = units.ToList();
        if (inputs.Count == 0) return ([], []);
        var (structures, diagnostics) = Arc.Compiler.PackageGenerator.Generators.ArcLayeredScopeTreeGenerator
            .GenerateUnitStructure(inputs, descriptor);
        var logs = diagnostics.ToList();
        var contexts = new List<ArcGinkgoUnitGenerationContext>();
        if (logs.Any(l => l.Level is Microsoft.Extensions.Logging.LogLevel.Error or Microsoft.Extensions.Logging.LogLevel.Critical))
            return (contexts, logs);
        var definitions = new Dictionary<uint, (ArcGinkgoUnitGenerationContext Context,
            Legacy.ArcScopeTreeFunctionNodeBase Source, ArcScopeTreeFunctionNodeBase Target)>();
        var pending = new Queue<uint>();
        foreach (var structure in structures)
        {
            var context = ArcGinkgoScopeAdapter.CreateContext(structure);
            contexts.Add(context);
            var functions = context.ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>();
            foreach (var source in structure.ScopeTree.GetNodes<Legacy.ArcScopeTreeFunctionNodeBase>())
            {
                var syntax = GetSyntax(source);
                if (syntax is null) continue;
                var target = functions.Single(f => ReferenceEquals(syntax, GetSyntax(f)));
                definitions.Add(target.Id, (context, source, target));
                if (deferredUnits?.Contains(structure.CompilationUnit) != true)
                    pending.Enqueue(target.Id);
            }
        }
        // Keep all declarations visible, but lower standard-library bodies only when called.
        var generated = new HashSet<uint>();
        while (pending.TryDequeue(out var id))
        {
            if (!generated.Add(id) || !definitions.TryGetValue(id, out var definition)) continue;
            var result = GenerateFunction(definition.Context, definition.Source, definition.Target);
            logs.AddRange(result.Logs);
            foreach (var call in result.Instructions.OfType<ArcControlFlowPrepareCallInstruction>())
                pending.Enqueue(call.FunctionSymbolId);
        }
        return (contexts, logs);
    }

    public static (ArcGinkgoUnitGenerationContext Context, IReadOnlyList<ArcCompilationLogBase> Logs)
        Generate(ArcCompilationUnitStructure structure)
    {
        var context = ArcGinkgoScopeAdapter.CreateContext(structure);
        var logs = new List<ArcCompilationLogBase>();
        var functions = context.ScopeTree.GetNodes<ArcScopeTreeFunctionNodeBase>();
        foreach (var source in structure.ScopeTree.GetNodes<Legacy.ArcScopeTreeFunctionNodeBase>())
        {
            var syntax = GetSyntax(source);
            if (syntax is null) continue;
            var target = functions.Single(f => ReferenceEquals(syntax, GetSyntax(f)));
            logs.AddRange(GenerateFunction(context, source, target).Logs);
        }
        return (context, logs);
    }

    private static ArcGinkgoInstructionGenerationResult GenerateFunction(ArcGinkgoUnitGenerationContext context,
        Legacy.ArcScopeTreeFunctionNodeBase source, ArcScopeTreeFunctionNodeBase target)
    {
        if (source.Annotations.Keys.Any(a => a.Signature is
            "NArc+NStd+NCompilation+ADeclaratorOnly" or "NArc+NCompilation+ADeclaratorOnly"))
        {
            target.GenerationResult = new Arc.Compiler.PackageGenerator.Models.Generation.ArcPartialGenerationResult();
            return new ArcGinkgoInstructionGenerationResult();
        }
        ArcBlockSequentialExecution body = source switch
        {
            Legacy.ArcScopeTreeIndividualFunctionNode n => n.SyntaxTree.Body,
            Legacy.ArcScopeTreeGroupFunctionNode n => n.SyntaxTree!.Body,
            Legacy.ArcScopeTreeLifecycleFunctionNode n => n.SyntaxTree.Body,
            _ => throw new InvalidOperationException()
        };
        return ArcGinkgoFunctionGenerator.Generate(context, target, body);
    }

    private static object? GetSyntax(Legacy.ArcScopeTreeFunctionNodeBase function) => function switch
    {
        Legacy.ArcScopeTreeIndividualFunctionNode n => n.SyntaxTree,
        Legacy.ArcScopeTreeGroupFunctionNode n => n.SyntaxTree,
        Legacy.ArcScopeTreeLifecycleFunctionNode n => n.SyntaxTree,
        _ => null
    };

    private static object? GetSyntax(ArcScopeTreeFunctionNodeBase function) => function switch
    {
        ArcScopeTreeIndividualFunctionNode n => n.SyntaxTree,
        ArcScopeTreeGroupFunctionNode n => n.SyntaxTree,
        ArcScopeTreeLifecycleFunctionNode n => n.SyntaxTree,
        _ => null
    };
}
