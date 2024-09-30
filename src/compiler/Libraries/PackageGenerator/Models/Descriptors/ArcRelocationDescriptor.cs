using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcRelocationDescriptor
    {
        public long Id { get; set; }

        public ArcRelocationType Type { get; set; }

        public long CommandBeginLocation { get; set; }

        public long Location { get; set; }

        public WeakReference<object>? Target { get; set; }
    }
}
