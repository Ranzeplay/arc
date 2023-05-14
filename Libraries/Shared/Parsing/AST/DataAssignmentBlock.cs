using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Expression;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public record DataAssignmentBlock(DataAccessor LhsTargetData, SimpleExpression RhsEvalExpression)
    {
    }
}
