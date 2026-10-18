namespace Arc.Compiler.PackageGenerator.Models.Relocation
{
    public class ArcRelocationLabel
    {
        public static ArcRelocationLabel Create(ArcRelocationLabelType type = ArcRelocationLabelType.BeginBlock)
            => new() { Layer = Guid.NewGuid(), Type = type };

        public string Name { get; set; } = "UNKNOWN";

        public ArcRelocationLabelType Type { get; set; }

        public long Location { get; set; }

        public required Guid Layer { get; set; }
    }
}
