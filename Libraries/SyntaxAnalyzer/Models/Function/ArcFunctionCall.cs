using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    internal class ArcFunctionCall
    {
        public ArcScopedIdentifier Identifier { get; set; }

        public IEnumerable<ArcFunctionCallArgument> Arguments { get; set; }
    }
}
