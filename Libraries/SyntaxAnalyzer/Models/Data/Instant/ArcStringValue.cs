namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant
{
    internal class ArcStringValue : ArcInstantValueBase
    {
        public string Value { get; set; }

        public bool Interpolation { get; set; }
    }
}
