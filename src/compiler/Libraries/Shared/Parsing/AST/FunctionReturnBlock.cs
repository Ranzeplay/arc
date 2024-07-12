using Arc.Compiler.Shared.Parsing.Components.Expression;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public record FunctionReturnBlock(SimpleExpression? Expression = null)
    {
    }
}
