using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Arc.Compiler.PackageGenerator.Models
{
    public class ArcGeneratorContext
    {
        public List<byte> GeneratedData { get; set; } = [];

        public Dictionary<long, ArcSymbolBase> Symbols { get; } = [];

        public List<ArcRelocationTarget> RelocationTargets { get; } = [];

        public List<ArcMaterializedRelocationTarget> MaterializedRelocationTargets { get; } = [];

        public List<ArcRelocationLabel> Labels { get; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public List<ArcConstant> Constants { get; } = [];

        public required ILogger Logger { get; set; }

        public void TransformLabelRelocationTargets()
        {
            for (var index = 0; index < RelocationTargets.Count; index++)
            {
                var target = RelocationTargets[index];
                // Skip non-label targets
                if (target.TargetType != ArcRelocationTargetType.Label) continue;

                // Replace label with concrete relative location
                List<ArcRelocationLabel> labelQuery;
                if (target.Parameter > 0)
                {
                    labelQuery = [..
                        Labels.Where(l => l.Location > target.Location)
                            .OrderBy(l => l.Location)
                        ];
                }
                else
                {
                    labelQuery = [..
                        Labels.Where(l => l.Location < target.Location)
                            .OrderByDescending(l => l.Location)
                        ];
                }

                // Parameter is 1-based
                var labelIndex = Utils.TraceRelocationLabel([.. labelQuery], target.Parameter) - 1;

                target.Parameter = target.Parameter > 0
                    ? labelIndex
                    : -labelIndex;

                RelocationTargets[index] = target;
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
                byte[] data;
                switch (target.TargetType)
                {
                    case ArcRelocationTargetType.Relative:
                        data = BitConverter.GetBytes(target.Parameter);
                        break;
                    case ArcRelocationTargetType.Absolute:
                        data = BitConverter.GetBytes(target.TargetLocation);
                        break;
                    case ArcRelocationTargetType.Symbol:
                        data = BitConverter.GetBytes(target.Symbol.Id);
                        break;
                    case ArcRelocationTargetType.Label:
                        data = BitConverter.GetBytes(target.Parameter);
                        break;
                    default:
                        throw new UnreachableException();
                }

                MaterializedRelocationTargets.Add(new(target.Location, data));
            }
        }

        public void ApplyFunctionSymbolRelocation()
        {
            foreach (var symbol in Symbols.Values)
            {
                if (symbol is ArcFunctionDescriptor desc)
                {
                    var label = Labels.First(l => l.Type == ArcRelocationLabelType.BeginFunction && l.Name == desc.RawFullName);
                    desc.EntrypointPos = label.Location;
                    Symbols[symbol.Id] = desc;
                }
            }
        }

        public void Append(ArcPartialGenerationResult result)
        {
            foreach (var symbol in result.OtherSymbols)
            {
                Symbols[symbol.Id] = symbol;
            }
            RelocationTargets.AddRange(result.RelocationTargets.Select(t =>
            {
                t.Location += GeneratedData.LongCount();
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

        public void Append(ArcCompilationUnitStructure structure)
        {
            foreach (var symbol in structure.Symbols)
            {
                Symbols[symbol.Id] = symbol;
            }
        }

        public void LoadPrimitiveTypes()
        {
            foreach (var bt in ArcPersistentData.BaseTypes)
            {
                Symbols.Add(bt.TypeId, bt);
            }
        }

        public ArcGenerationSource GenerateSource()
        {
            return GenerateSource([]);
        }

        public ArcGenerationSource GenerateSource(IEnumerable<IArcLocatable> location)
        {
            return new()
            {
                AccessibleSymbols = Symbols.Values,
                PackageDescriptor = PackageDescriptor,
                ParentSignature = new ArcSignature() { Locators = location }
            };
        }
    }
}
