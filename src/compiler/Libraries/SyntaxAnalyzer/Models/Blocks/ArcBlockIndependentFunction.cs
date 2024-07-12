using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockIndependentFunction : ArcFunctionBase
    {
        public ArcBlockIndependentFunction(ArcSourceCodeParser.Arc_function_blockContext source)
        {
            Declarator = new ArcFunctionDeclarator(source.arc_function_declarator());
            Body = new ArcFunctionBody(source.arc_wrapped_function_body());
        }
    }
}
