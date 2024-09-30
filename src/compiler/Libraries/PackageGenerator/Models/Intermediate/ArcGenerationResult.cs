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
            RelocationDescriptors = RelocationDescriptors.Concat(result.RelocationDescriptors.Select(x =>
            {
                x.Location += GeneratedData.Count();
                return x;
            }));
            Labels = Labels.Concat(result.Labels.Select(x =>
            {
                x.Position += GeneratedData.Count();
                return x;
            }));

            return this;
        }

        public void ClearDataSlots()
        {
            Symbols = Symbols.Where(x => x.Value is not ArcDataSlot).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
