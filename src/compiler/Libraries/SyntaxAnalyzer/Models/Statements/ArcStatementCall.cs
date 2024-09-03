using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    public class ArcStatementCall(ArcSourceCodeParser.Arc_stmt_callContext context) : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_callContext>
    {
        public ArcFunctionCall FunctionCall { get; set; } = new(context.arc_function_call_base());
        public ArcSourceCodeParser.Arc_stmt_callContext Context { get; } = context;
    }
}
