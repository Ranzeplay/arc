namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant
{
    public class ArcInstantDecimalValue(decimal value)
    {
        public decimal Value { get; set; } = value;
    }
}
