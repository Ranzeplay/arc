namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant
{
    public class ArcInstantIntegerValue(long value)
    {
        public long Value { get; set; } = value;
    }
}
