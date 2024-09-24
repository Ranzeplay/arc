using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models
{
    internal class ArcPartialEncodeResult
    {
        public IEnumerable<byte> Bytes { get; set; }

        public IEnumerable<ArcRelocationDescriptor> RelocationDescriptors { get; set; }

        public IEnumerable<ArcLabel> Labels { get; set; }
    }
}
