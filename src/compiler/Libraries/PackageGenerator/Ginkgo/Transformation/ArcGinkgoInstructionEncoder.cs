using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

/// <summary>Encodes instructions and records byte positions for function-local relocation.</summary>
public static class ArcGinkgoInstructionEncoder
{
    public static (ArcPartialGenerationResult Result, IReadOnlySet<long> Boundaries) Encode(
        ArcGinkgoInstructionGenerationResult input)
    {
        var result = ArcPartialGenerationResult.WithLogs(input.Logs);
        var boundaries = new HashSet<long>();
        if (result.DiscardResult)
        {
            return (result, boundaries);
        }
        
        if (input.RelocationTargets.Count > 0 || input.RelocationLabels.Count > 0)
        {
            result.Logs.Add(new ArcSourceUnlocatableLog(LogLevel.Error, 0,
                "Ginkgo relocations must be attached to jump and label instructions", "Ginkgo instruction encoder"));
            return (result, boundaries);
        }
        
        foreach (var instruction in input.Instructions)
        {
            var encoded = instruction.Encode();
            var offset = result.GeneratedData.Count;
            boundaries.Add(offset);
            foreach (var target in encoded.RelocationTargets)
            {
                target.Location += offset;
            }
            foreach (var label in encoded.RelocationLabels)
            {
                label.Location += offset;
            }
            
            result.GeneratedData.AddRange(encoded.GeneratedData);
            result.RelocationTargets.AddRange(encoded.RelocationTargets);
            result.RelocationLabels.AddRange(encoded.RelocationLabels);
        }
        return (result, boundaries);
    }
}
