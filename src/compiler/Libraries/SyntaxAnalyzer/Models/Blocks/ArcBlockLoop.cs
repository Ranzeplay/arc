using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    public class ArcBlockLoop(ArcSourceCodeParser.Arc_stmt_loopContext context) : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_loopContext>
    {
        public ArcBlockSequentialExecution Body { get; set; } = new(context.arc_wrapped_function_body());
        public ArcSourceCodeParser.Arc_stmt_loopContext Context { get; } = context;
    }
}
