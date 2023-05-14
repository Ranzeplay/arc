using Arc.Compiler.Shared.LexicalAnalysis;

namespace Arc.Compiler.Shared.Parsing.Components.Expression
{
    public record RelationalExpression(SimpleExpression LhsExpression, RelationOperatorType Relation, SimpleExpression RhsExpression)
    {
    }
}
