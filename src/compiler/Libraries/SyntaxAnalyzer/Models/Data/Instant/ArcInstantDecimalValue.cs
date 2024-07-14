namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant
{
    internal class ArcInstantDecimalValue(decimal value)
    {
        public decimal Value { get; set; } = value;
    }
}
