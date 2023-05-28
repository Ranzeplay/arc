using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Expression;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public record ConditionalLoopBlock : ConditionalBlock
    {
        public ConditionalLoopBlock(SimpleExpression Condition, ActionBlock Actions)
            : base(Condition, Actions)
        {
        }
    }
}
