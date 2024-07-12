using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementCall(ArcSourceCodeParser.Arc_stmt_callContext context) : ArcFunctionCall(context.arc_function_call_base())
    {
    }
}
