using System.Text;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    public class ArcPackageDescriptor
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();

        public long Version { get; set; }

        public ArcPackageType Type { get; set; }

        public ulong EntrypointFunctionId { get; set; }

        public ulong DataAlignmentLength { get; set; }

        public ulong RootFunctionTableEntryPos { get; set; }

        public ulong RootConstantTableEntryPos { get; set; }

        public ulong RootGroupTableEntryPos { get; set; }

        public ulong RegionTableEntryPos { get; set; }

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
