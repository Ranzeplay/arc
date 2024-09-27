using Arc.Compiler.PackageGenerator.Models.Descriptors;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ArcGenerationSource<T>
    {
        public T Value { get; set; }

        public Dictionary<long, object> Symbols { get; set; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public IEnumerable<ArcDataSlot> DataSlots { get; set; }
    }
}
