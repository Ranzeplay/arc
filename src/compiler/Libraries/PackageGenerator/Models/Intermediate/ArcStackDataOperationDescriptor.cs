using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal partial class ArcStackDataOperationDescriptor(ArcDataSourceType source, ulong locationId, bool overwrite)
    {
        public ArcDataSourceType Source { get; set; } = source;

        public ulong LocationId { get; set; } = locationId;

        public bool Overwrite { get; set; } = overwrite;
    }
}
