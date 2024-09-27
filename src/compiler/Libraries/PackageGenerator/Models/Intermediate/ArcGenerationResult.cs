using Arc.Compiler.PackageGenerator.Models.Descriptors;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ArcGenerationResult
    {
        public IEnumerable<byte> GeneratedData { get; set; } = [];

        public Dictionary<long, object> Symbols { get; set; } = [];

        public IEnumerable<ArcRelocationDescriptor> RelocationDescriptors { get; set; } = [];

        public IEnumerable<ArcLabel> Labels { get; set; } = [];

        public ArcGenerationResult Append(ArcGenerationResult result)
        {
            GeneratedData = GeneratedData.Concat(result.GeneratedData);
            foreach (var item in result.Symbols)
            {
                Symbols.Add(item.Key, item.Value);
            }
            RelocationDescriptors = RelocationDescriptors.Concat(result.RelocationDescriptors);
            Labels = Labels.Concat(result.Labels);

            return this;
        }
    }
}
