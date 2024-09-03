using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    public class ArcFunctionCallArgument(ArcSourceCodeParser.Arc_expressionContext context)
    {
        public ArcExpression Expression { get; set; } = new ArcExpression(context);
    }
}
