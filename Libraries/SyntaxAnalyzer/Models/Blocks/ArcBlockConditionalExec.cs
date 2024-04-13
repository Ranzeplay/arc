namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal class ArcBlockConditionalExec
    {
        public ArcBlockIf IfBlock { get; set; }

        public List<ArcBlockElseIf> ElseIfBlocks { get; set; }

        public ArcBlockElse ElseBlock { get; set; }
    }
}
