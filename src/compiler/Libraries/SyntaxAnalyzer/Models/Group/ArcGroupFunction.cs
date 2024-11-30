using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group
{
    public class ArcGroupFunction : ArcFunctionBase<ArcSourceCodeParser.Arc_group_functionContext>
    {
        public ArcGroupFunction(ArcSourceCodeParser.Arc_group_functionContext context) : base()
        {
            Declarator = new(context.arc_function_block().arc_function_declarator());
            Body = new(context.arc_function_block().arc_wrapped_function_body());

            Context = context;
        }
    }
}
