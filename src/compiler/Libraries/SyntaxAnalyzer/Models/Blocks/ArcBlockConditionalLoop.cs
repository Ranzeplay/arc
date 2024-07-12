using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockConditionalLoop(ArcSourceCodeParser.Arc_conditional_loop_blockContext context) : ArcBlockConditionalBase(context.arc_conditional_block())
    {
    }
}
