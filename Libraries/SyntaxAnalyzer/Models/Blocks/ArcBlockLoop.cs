using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockLoop(ArcSourceCodeParser.Arc_loop_blockContext context) : ArcBlockSequentialExecution(context.arc_wrapped_body())
    {
    }
}
