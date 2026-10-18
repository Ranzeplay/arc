using Antlr4.Runtime;
using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

public class ArcGinkgoInstructionGenerationResult
{
    public List<ArcGinkgoInstructionBase> Instructions { get; set; } = [];
    
    public List<ArcRelocationTarget> RelocationTargets { get; init; } = [];

    public List<ArcRelocationLabel> RelocationLabels { get; init; } = [];
    
    public List<ArcCompilationLogBase> Logs { get; init; } = [];
    
    public bool DiscardResult => Logs.Any(l => l.Level is LogLevel.Error or LogLevel.Critical);
    
    public ArcImplicitRegister? OutImplicitRegister { get; set; }

    // Set only when this invocation produces OutImplicitRegister; void lifecycle calls do not.
    public ArcControlFlowInvokeInstruction? OutputInvoke { get; set; }

    public void StoreOutput(ArcImplicitRegister target)
    {
        var source = OutImplicitRegister ?? throw new InvalidOperationException("No output register to store");
        // Only redirect the final producer. Earlier calls may feed arguments or field receivers.
        if (OutputInvoke is { } invoke && ReferenceEquals(Instructions.LastOrDefault(), invoke) && invoke.Dest == source.Id)
            invoke.Dest = target.Id;
        else if (source.Id != target.Id)
        {
            Instructions.Add(new ArcDataCopyInstruction { Src = source.Id, Dest = target.Id });
            OutputInvoke = null;
        }
        OutImplicitRegister = target;
    }

    public void Append(ArcGinkgoInstructionGenerationResult res)
    {
        Instructions.AddRange(res.Instructions);
        RelocationTargets.AddRange(res.RelocationTargets);
        RelocationLabels.AddRange(res.RelocationLabels);
        Logs.AddRange(res.Logs);
    }

    public static ArcGinkgoInstructionGenerationResult WithLog(LogLevel level, uint code, string message, string sourceFile, IArcTraceable<ParserRuleContext> locatable)
    {
        var res = new ArcGinkgoInstructionGenerationResult();
        res.Logs.Add(new ArcSourceLocatableLog(level, code, message, sourceFile, locatable));
        return res;
    }
    
    public static ArcGinkgoInstructionGenerationResult WithLog(LogLevel level, uint code, string message, string sourceFile, ParserRuleContext context)
    {
        var res = new ArcGinkgoInstructionGenerationResult();
        res.Logs.Add(new ArcSourceLocatableLog(level, code, message, sourceFile, context));
        return res;
    }

    public static ArcGinkgoInstructionGenerationResult WithLog(LogLevel level, uint code, string message, string sourceFile)
    {
        var res = new ArcGinkgoInstructionGenerationResult();
        res.Logs.Add(new ArcSourceUnlocatableLog(level, code, message, sourceFile));
        return res;
    }
    
    public static ArcGinkgoInstructionGenerationResult WithLogs(IEnumerable<ArcCompilationLogBase> logs)
    {
        return new ArcGinkgoInstructionGenerationResult
        {
            Logs = logs.ToList()
        };
    }
}
