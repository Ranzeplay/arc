using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockIf
    {
        public IEnumerable<ArcBlockConditional> ConditionalBlocks { get; set; }

        public ArcFunctionBody? ElseBody { get; set; }

        public ArcBlockIf(ArcSourceCodeParser.Arc_stmt_ifContext source)
        {
            var list = new List<ArcBlockConditional>();
            for(var i = 0; i < source.arc_expression().Length; i++)
            {
                list.Add(new(source.arc_expression()[i], source.arc_wrapped_function_body()[i]));
            }
        }
    }
}
