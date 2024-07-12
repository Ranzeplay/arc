namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal abstract class ArcIdentifierBase
    {
        protected IList<string> Names { get; set; } = ["", "", ""];
    }
}
