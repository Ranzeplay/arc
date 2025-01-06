namespace Arc.Compiler.PackageGenerator.Models.Relocation
{
    public class ArcMaterializedRelocationTarget(long location, byte[] data)
    {
        public long Location { get; } = location;

        public byte[] Data { get; } = data;
    }
}
