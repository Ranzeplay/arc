using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models
{
    internal class ArcEncodeResult
    {
        public IEnumerable<byte> Data { get; set; }

        public IEnumerable<ArcRelocationType> RelocationTargets { get; set; }
    }
}
