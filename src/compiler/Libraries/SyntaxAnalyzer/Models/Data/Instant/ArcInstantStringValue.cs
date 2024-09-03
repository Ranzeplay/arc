namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant
{
    public class ArcInstantStringValue(string value, bool interpolation)
    {
        public string Value { get; set; } = value;

        public bool Interpolation { get; set; } = interpolation;
    }
}
