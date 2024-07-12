using Arc.Compiler.Shared.Parsing.Components.Expression;

namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public record FunctionArgument(FunctionParameter Declarator, SimpleExpression EvaluateExpression)
    {
    }
}
