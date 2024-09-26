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
            return this;
        }
    }
}
