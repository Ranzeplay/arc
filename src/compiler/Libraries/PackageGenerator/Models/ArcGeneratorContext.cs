using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.SyntaxAnalyzer.Models;

namespace Arc.Compiler.PackageGenerator.Models
{
    internal class ArcGeneratorContext
    {
        public IEnumerable<byte> GeneratedData { get; set; } = [];

        public Dictionary<long, object> Symbols { get; set; } = [];

        public IEnumerable<ArcRelocationDescriptor> RelocationDescriptors { get; set; } = [];

        public IEnumerable<ArcLabel> Labels { get; set; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public void ApplyReloation() { }

        public void Append(ArcGeneratorContext result) { }

        public void Append(ArcGenerationResult result)
        {
            GeneratedData = GeneratedData.Concat(result.GeneratedData);
            foreach (var symbol in result.Symbols)
            {
                Symbols[symbol.Key] = symbol.Value;
            }
            RelocationDescriptors = RelocationDescriptors.Concat(result.RelocationDescriptors);
            Labels = Labels.Concat(result.Labels);
        }

        public void LoadFromCompilationUnit(ArcCompilationUnit compilationUnit) { }

        public void LoadPrimitiveTypes()
        {
            foreach (var bt in PersistentData.BaseTypes)
            {
                Symbols.Add(bt.TypeId, bt);
            }
        }

        public ArcGenerationSource<T> GenerateSource<T>(T value)
        {
            return new()
            {
                Value = value,
                Symbols = Symbols,
            };
        }
    }
}
