using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    public class ArcBlockConditional(ArcSourceCodeParser.Arc_expressionContext expressionContext, ArcSourceCodeParser.Arc_wrapped_function_bodyContext functionBlockContext)
    {
        public ArcExpression Expression { get; set; } = new ArcExpression(expressionContext);

        public ArcBlockSequentialExecution Body { get; set; } = new ArcBlockSequentialExecution(functionBlockContext);
    }
}
