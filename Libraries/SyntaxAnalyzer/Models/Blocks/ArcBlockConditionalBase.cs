using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks
{
    internal abstract class ArcBlockConditionalBase
    {
        public ArcExpression Expression { get; set; }

        public ArcBlockSequentialExecution Body { get; set; }
    }
}
