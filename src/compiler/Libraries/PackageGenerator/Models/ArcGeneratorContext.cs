using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.PackageGenerator.Models
{
    public class ArcGeneratorContext
    {
        public IEnumerable<byte> GeneratedData { get; set; } = [];

        public Dictionary<long, ArcSymbolBase> Symbols { get; set; } = [];

        public IEnumerable<ArcRelocationTarget> RelocationTargets { get; set; } = [];

        public IEnumerable<ArcRelocationLabel> Labels { get; set; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public IEnumerable<ArcConstant> Constants { get; set; } = [];

        public void TransformLabelRelocationTargets()
        {
            var targets = RelocationTargets.ToList();

            for (var index = 0; index < targets.Count; index++)
            {
                var target = targets[index];
                // Skip non-label targets
                if (target.TargetType != ArcRelocationTargetType.Label) continue;
                
                // Replace label with concrete relative location
                IEnumerable<ArcRelocationLabel> labelQuery;
                if (target.Parameter > 0)
                {
                    labelQuery = Labels.Where(l => l.Location > target.Location)
                        .OrderBy(l => l.Location);
                }
                else
                {
                    labelQuery = Labels.Where(l => l.Location < target.Location)
                        .OrderByDescending(l => l.Location);
                }

                // Parameter is 1-based
                var label = labelQuery.ElementAt(Utils.TraceRelocationLabel(labelQuery, target.Parameter) - 1);

                target.TargetType = ArcRelocationTargetType.Relative;
                target.Parameter = label.Location - target.Location;

                targets[index] = target;
            }
        }

        public void ApplyRelocation() {
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
                    default:
                        throw new InvalidOperationException();
                }

                GeneratedData = GeneratedData.Take((int) target.Location)
                    .Concat(data)
                    .Concat(GeneratedData.Skip((int)target.Location + data.Length));
            }
        }

        public void Append(ArcGeneratorContext result) { }

        public void Append(ArcPartialGenerationResult result)
        {
            foreach (var symbol in result.OtherSymbols)
            {
                Symbols[symbol.Id] = symbol;
            }
            RelocationTargets = RelocationTargets.Concat(result.RelocationTargets.Select(t =>
            {
                t.Location += GeneratedData.LongCount();
                return t;
            }));
            Labels = Labels.Concat(result.RelocationLabels.Select(l =>
            {
                l.Location += GeneratedData.LongCount();
                return l;
            }));
            Constants = Constants.Concat(result.AddedConstants);
            GeneratedData = GeneratedData.Concat(result.GeneratedData);
        }

        public void Append(ArcCompilationUnitStructure structure)
        {
            foreach (var symbol in structure.Symbols)
            {
                Symbols[symbol.Id] = symbol;
            }
        }

        public void LoadFromCompilationUnit(ArcCompilationUnit compilationUnit) { }

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
