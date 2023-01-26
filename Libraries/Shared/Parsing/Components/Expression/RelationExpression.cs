using Arc.Compiler.Shared.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Expression
{
    public class RelationalExpression
    {
        public SimpleExpression LhsExpression { get; }

        public RelationOperatorType Relation { get; }

        public SimpleExpression RhsExpression { get; }

        public RelationalExpression(SimpleExpression lhsExpression, RelationOperatorType relation, SimpleExpression rhsExpression)
        {
            LhsExpression = lhsExpression;
            Relation = relation;
            RhsExpression = rhsExpression;
        }
    }
}
