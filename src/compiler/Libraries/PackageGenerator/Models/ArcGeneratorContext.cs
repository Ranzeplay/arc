using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
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

        public void ApplyReloation() { }

        public void Append(ArcGeneratorContext result) { }

        public void Append(ArcPartialGenerationResult result)
        {
            GeneratedData = GeneratedData.Concat(result.GeneratedData);
            foreach (var symbol in result.OtherSymbols)
            {
                Symbols[symbol.Id] = symbol;
            }
            RelocationTargets = RelocationTargets.Concat(result.RelocationTargets);
            Labels = Labels.Concat(result.RelocationLabels);
            Constants = Constants.Concat(result.AddedConstants);
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
