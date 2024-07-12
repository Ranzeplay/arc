using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementCall(ArcSourceCodeParser.Arc_call_stmtContext context) : ArcFunctionCall(context.arc_function_call())
    {
    }
}
