using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models
{
    internal class ArcGeneratorContext
    {
        public IEnumerable<byte> GeneratedData { get; set; }

        public Dictionary<Guid, WeakReference<object>> Symbols { get; set; }

        public IEnumerable<ArcRelocationDescriptor> RelocationDescriptors { get; set; }

        public IEnumerable<ArcLabel> Labels { get; set; }

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public void ApplyReloation() { }

        public void Append(ArcPartialEncodeResult result) { }
    }
}
