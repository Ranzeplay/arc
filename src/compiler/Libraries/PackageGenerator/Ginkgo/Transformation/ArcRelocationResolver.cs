using System.Buffers.Binary;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

/// <summary>Validates and patches function-local jumps in encoded instruction bytes.</summary>
public static class ArcRelocationResolver
{
    public static void Resolve(ArcPartialGenerationResult result, IReadOnlySet<long> boundaries)
    {
        if (result.DiscardResult) return;
        var labels = result.RelocationLabels.GroupBy(l => (l.Layer, l.Type)).ToList();
        if (labels.Any(g => g.Count() != 1))
        {
            Fail("Duplicate Ginkgo relocation label");
            return;
        }
        var lookup = labels.ToDictionary(g => g.Key, g => g.Single().Location);
        var bytes = result.GeneratedData.ToArray();
        foreach (var target in result.RelocationTargets)
        {
            long destination;
            switch (target.TargetType)
            {
                case ArcRelocationTargetType.Label:
                    if (!lookup.TryGetValue((target.Layer, target.Label), out destination))
                    {
                        Fail("Unresolved Ginkgo relocation label");
                        continue;
                    }
                    destination = checked(destination + target.Parameter);
                    break;
                case ArcRelocationTargetType.Absolute:
                    destination = checked(target.TargetLocation + target.Parameter);
                    break;
                case ArcRelocationTargetType.Relative:
                    destination = checked(target.Location + sizeof(long) + target.Offset + target.Parameter);
                    break;
                default:
                    Fail("Ginkgo jumps require function-local addresses, not legacy symbols");
                    continue;
            }
            if (!boundaries.Contains(destination))
            {
                Fail("Ginkgo jump destination is not an instruction boundary in this function");
                continue;
            }
            BinaryPrimitives.WriteInt64LittleEndian(bytes.AsSpan(checked((int)target.Location), sizeof(long)), destination);
        }
        if (!result.DiscardResult)
        {
            result.GeneratedData.Clear();
            result.GeneratedData.AddRange(bytes);
            result.RelocationTargets.Clear();
        }

        void Fail(string message)
        {
            result.Logs.Add(new ArcSourceUnlocatableLog(LogLevel.Error, 0, message, "Ginkgo relocation resolver"));
            result.GeneratedData.Clear();
        }
    }
}
