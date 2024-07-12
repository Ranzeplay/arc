using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function
{
    internal class ArcFunctionBody(ArcSourceCodeParser.Arc_wrapped_function_bodyContext context) : ArcBlockSequentialExecution(context)
    {
    }
}
