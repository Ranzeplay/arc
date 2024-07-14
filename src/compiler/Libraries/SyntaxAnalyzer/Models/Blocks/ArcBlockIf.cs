using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockIf
    {
        public IEnumerable<ArcBlockConditional> ConditionalBlocks { get; set; }

        public ArcFunctionBody? ElseBody { get; set; }

        public ArcBlockIf(ArcSourceCodeParser.Arc_stmt_ifContext context)
        {
            var list = new List<ArcBlockConditional>();
            for (var i = 0; i < context.arc_expression().Length; i++)
            {
                list.Add(new(context.arc_expression()[i], context.arc_wrapped_function_body()[i]));
            }
            ConditionalBlocks = list;

            if (context.arc_wrapped_function_body().Length > context.arc_expression().Length)
            {
                ElseBody = new(context.arc_wrapped_function_body()[^1]);
            }
        }
    }
}
