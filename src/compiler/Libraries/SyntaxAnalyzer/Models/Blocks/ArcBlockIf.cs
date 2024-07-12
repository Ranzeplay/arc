using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockIf(ArcSourceCodeParser.Arc_if_blockContext context) : ArcBlockConditionalBase(context.arc_conditional_block())
    {
    }
}
