using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockLoop(ArcSourceCodeParser.Arc_stmt_loopContext context) : ArcExecutionStepBase
    {
        public ArcBlockSequentialExecution Body { get; set; } = new(context.arc_wrapped_function_body());
    }
}
