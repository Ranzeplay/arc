using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal abstract class ArcBlockConditionalBase(ArcSourceCodeParser.Arc_conditional_blockContext context)
    {
        public ArcExpression Expression { get; set; } = new ArcExpression(context.arc_expression());

        public ArcBlockSequentialExecution Body { get; set; } = new ArcBlockSequentialExecution(context.arc_wrapped_body());
    }
}
