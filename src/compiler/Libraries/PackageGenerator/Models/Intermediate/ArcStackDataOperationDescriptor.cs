using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal partial class ArcStackDataOperationDescriptor(ArcDataSourceType source, ArcMemoryStorageType storageType, long locationId, bool overwrite)
    {
        public ArcDataSourceType Source { get; set; } = source;

        public ArcMemoryStorageType StorageType { get; set; } = storageType;

        public long LocationId { get; set; } = locationId;

        public bool Overwrite { get; set; } = overwrite;
    }
}
