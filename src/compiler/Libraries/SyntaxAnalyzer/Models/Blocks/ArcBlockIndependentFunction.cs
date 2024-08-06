using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockIndependentFunction : ArcFunctionBase<ArcSourceCodeParser.Arc_function_blockContext>
    {
        public ArcBlockIndependentFunction(ArcSourceCodeParser.Arc_function_blockContext context)
        {
            Context = context;
            Declarator = new ArcFunctionDeclarator(context.arc_function_declarator());
            Body = new ArcFunctionBody(context.arc_wrapped_function_body());
        }
    }
}
