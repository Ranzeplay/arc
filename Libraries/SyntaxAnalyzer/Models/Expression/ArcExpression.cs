using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Expression
{
    internal class ArcExpression(ArcSourceCodeParser.Arc_expressionContext source)
    {
        public IEnumerable<ArcExpressionTermBase> Terms { get; set; }
    }
}
