using System.Text;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    public class ArcPackageDescriptor
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();

        public long Version { get; set; }

        public ArcPackageType Type { get; set; }

        public long EntrypointFunctionId { get; set; }

        public long DataAlignmentLength { get; set; }

        public long RootFunctionTableEntryPos { get; set; }

        public long RootConstantTableEntryPos { get; set; }

        public long RootGroupTableEntryPos { get; set; }

        public long RegionTableEntryPos { get; set; }

        // Call after name being set.
        public long GetLength()
        {
            // 1 byte for type
            var result = 1;
            // 4 bytes for name length
            // n bytes for name
            result += 4 + Encoding.UTF8.GetByteCount(Name);
            // 8 bytes for version
            // 8 bytes for entrypoint function
            // 8 bytes for data alignment
            // 8 bytes for root function table entry
            // 8 bytes for root constant table entry
            // 8 bytes for root group table entry
            // 8 bytes for region table entry
            result += 8 * 7;
            return result;
        }
    }
}
