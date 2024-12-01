using System.Text;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    public class ArcPackageDescriptor
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public ArcPackageType Type { get; set; }

        public long EntrypointFunctionId { get; set; }

        public long DataAlignmentLength { get; set; }

        public long RootFunctionTableEntryPos { get; set; }

        public long RootConstantTableEntryPos { get; set; }

        public long RootGroupTableEntryPos { get; set; }

        public long RegionTableEntryPos { get; set; }
    }
}
