using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Encoders;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Arc.Compiler.PackageGenerator.Models
{
    public class ArcGeneratorContext
    {
        public ArcScopeTree GlobalScopeTree { get; set; }

        public List<byte> GeneratedData { get; set; } = [];

        public Dictionary<long, ArcSymbolBase> Symbols =>
            GlobalScopeTree.FlattenedNodes
                .SelectMany(n => n.GetSymbols())
                .ToDictionary(s => s.Id);

        public List<ArcRelocationTarget> RelocationTargets { get; } = [];

        public List<ArcMaterializedRelocationTarget> MaterializedRelocationTargets { get; } = [];

        public List<ArcRelocationLabel> Labels { get; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public List<ArcConstant> Constants { get; } = [];

        public required ILogger Logger { get; set; }

        public void TransformLabelRelocationTargets()
        {
            for (var i = 0; i < RelocationTargets.Count; i++)
            {
                var target = RelocationTargets[i];
                // Skip non-label targets
                if (target.TargetType != ArcRelocationTargetType.Label) continue;

                target.Parameter = Utils.LocateLabelRelativeLocation(Labels, target.Parameter > 0, target, Math.Abs(target.Parameter));

                RelocationTargets[i] = target;
            }
        }

        public void ApplyRelocation()
        {
            foreach (var target in MaterializedRelocationTargets)
            {
                GeneratedData.ReplaceRange((int)target.Location, target.Data);
            }
        }

        public void PreApplyRelocation()
        {
            foreach (var target in RelocationTargets)
            {
                byte[] data = target.TargetType switch
                {
                    ArcRelocationTargetType.Relative => BitConverter.GetBytes(target.Parameter),
                    ArcRelocationTargetType.Absolute => BitConverter.GetBytes(target.TargetLocation),
                    ArcRelocationTargetType.Symbol => BitConverter.GetBytes(target.Symbol.Id),
                    ArcRelocationTargetType.Label => BitConverter.GetBytes(target.Parameter),
                    _ => throw new UnreachableException(),
                };
                MaterializedRelocationTargets.Add(new(target.Location, data));
            }
        }

        public void Append(ArcPartialGenerationResult result)
        {
            RelocationTargets.AddRange(result.RelocationTargets.Select(t =>
            {
                t.Location += GeneratedData.Count;
                return t;
            }));
            Labels.AddRange(result.RelocationLabels.Select(l =>
            {
                l.Location += GeneratedData.Count;
                return l;
            }));
            Constants.AddRange(result.AddedConstants);
            GeneratedData.AddRange(result.GeneratedData);
        }

        public void Append(ArcGeneratorContext context)
        {
            context.Labels.ForEach(l =>
            {
                l.Location += GeneratedData.Count;
                Labels.Add(l);
            });

            context.RelocationTargets.ForEach(t =>
            {
                t.Location += GeneratedData.Count;
                RelocationTargets.Add(t);
            });

            Constants.AddRange(context.Constants);
            GeneratedData.AddRange(context.GeneratedData);
        }

        public ArcGenerationSource GenerateSource()
        {
            return GenerateSource([], GlobalScopeTree.Root);
        }

        public ArcGenerationSource GenerateSource(IEnumerable<IArcLocatable> location, ArcScopeTreeNodeBase node)
        {
            return new()
            {
                PackageDescriptor = PackageDescriptor,
                GlobalScopeTree = GlobalScopeTree,
                ParentSignature = new ArcSignature() { Locators = location.ToList() },
                CurrentNode = node
            };
        }

        public IEnumerable<byte> DumpFullByteStream()
        {
            Logger.LogInformation("Dumping full byte stream for {}", PackageDescriptor.Name);

            var result = new List<byte>();

            result.AddRange([0x20, 0x24]);

            result.AddRange(ArcPackageDescriptorEncoder.Encode(this));
            result.AddRange(ArcSymbolTableEncoder.Encode(this));
            result.AddRange(ArcConstantTableEncoder.Encode(this));
            TransformLabelRelocationTargets();
            PreApplyRelocation();
            ApplyRelocation();
            result.AddRange(GeneratedData);

            Logger.LogTrace("Generated bytes: {}", BitConverter.ToString([.. GeneratedData]).Replace("-", " "));
            Logger.LogInformation("Generated {} bytes in total", GeneratedData.Count);

            return result;
        }
    }
}
