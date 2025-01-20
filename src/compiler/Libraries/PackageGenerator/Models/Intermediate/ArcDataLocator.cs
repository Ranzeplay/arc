using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal partial class ArcDataLocator(ArcDataSourceType source, long locationId, IEnumerable<ArcGroupFieldDescriptor> fieldChain, IEnumerable<byte> addend)
    {
        public ArcDataSourceType Source { get; set; } = source;

        public long LocationId { get; set; } = locationId;

        public List<ArcGroupFieldDescriptor> FieldChain { get; set; } = fieldChain.ToList();

        public IEnumerable<byte> Addend { get; set; } = addend;
    }
}
