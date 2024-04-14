using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockElseIf(ArcSourceCodeParser.Arc_elif_blockContext context) : ArcBlockConditionalBase(context.arc_conditional_block())
    {
    }
}
