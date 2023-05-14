using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Expression;

namespace Arc.Compiler.Shared.Parsing.Components
{
    public record ConditionalBlock(RelationalExpression Condition, ActionBlock Actions)
    {
    }
}
