using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementCall(ArcSourceCodeParser.Arc_stmt_callContext context) : ArcExecutionStepBase
    {
        public ArcFunctionCall FunctionCall { get; set; } = new(context.arc_function_call_base());
    }
}
