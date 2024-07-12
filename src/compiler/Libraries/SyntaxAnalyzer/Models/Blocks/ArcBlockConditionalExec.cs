using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockConditionalExec(ArcSourceCodeParser.Arc_conditional_exec_blockContext context)
    {
        public ArcBlockIf IfBlock { get; set; } = new ArcBlockIf(context.arc_if_block());

        public IEnumerable<ArcBlockElseIf> ElseIfBlocks { get; set; } = context.arc_elif_block().Select(elseIfBlock => new ArcBlockElseIf(elseIfBlock));

        public ArcBlockElse ElseBlock { get; set; } = new ArcBlockElse(context.arc_else_block());
    }
}
