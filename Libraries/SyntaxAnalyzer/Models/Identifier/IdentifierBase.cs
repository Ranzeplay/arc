namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal abstract class IdentifierBase
    {
        protected IList<string> Names { get; set; } = ["", "", ""];
    }
}
