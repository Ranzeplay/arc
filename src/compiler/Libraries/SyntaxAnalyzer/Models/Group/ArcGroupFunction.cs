using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group
{
    public class ArcGroupFunction : ArcNamedFunction<ArcSourceCodeParser.Arc_group_functionContext>
    {
        public ArcGroupFunction(ArcSourceCodeParser.Arc_group_functionContext context)
        {
            Declarator = new(context.arc_function_block().arc_function_declarator(), true);
            Body = new(context.arc_function_block().arc_wrapped_function_body());

            Context = context;
        }

        public static explicit operator ArcBlockIndependentFunction(ArcGroupFunction groupFn)
        {
            return new ArcBlockIndependentFunction(groupFn.Context.arc_function_block())
            {
                Declarator = groupFn.Declarator,
                Body = groupFn.Body,
            };
        }
    }
}
