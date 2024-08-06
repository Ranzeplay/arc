using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockConditionalLoop(ArcSourceCodeParser.Arc_stmt_whileContext context) : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_whileContext>
    {
        public ArcBlockConditional ConditionalBlock { get; set; } = new(context.arc_expression(), context.arc_wrapped_function_body());
        public ArcSourceCodeParser.Arc_stmt_whileContext Context { get; } = context;
    }
}
