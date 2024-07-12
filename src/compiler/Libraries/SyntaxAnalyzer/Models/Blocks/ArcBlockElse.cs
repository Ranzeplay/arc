using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockElse(ArcSourceCodeParser.Arc_else_blockContext context) : ArcBlockSequentialExecution(context.arc_wrapped_body())
    {
    }
}
