using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockConditionalLoop(ArcSourceCodeParser.Arc_stmt_whileContext context) : ArcBlockConditional(context.arc_expression(), context.arc_wrapped_function_body())
    {
    }
}
