namespace Arc.Compiler.PackageGenerator.Models.Relocation
{
    public class ArcRelocationLabel
    {
        public string Name { get; set; } = "UNKNOWN";

        public ArcRelocationLabelType Type { get; set; }

        public long Location { get; set; }

        public required Guid Layer { get; set; }
    }
}
