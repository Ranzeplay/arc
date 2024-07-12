using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockLoop(ArcSourceCodeParser.Arc_stmt_loopContext context) : ArcBlockSequentialExecution(context.arc_wrapped_function_body())
    {
    }
}
