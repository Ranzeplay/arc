using Arc.Compiler.Shared.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Expression
{
    public record RelationalExpression(SimpleExpression LhsExpression, RelationOperatorType Relation, SimpleExpression RhsExpression)
    {
    }
}
