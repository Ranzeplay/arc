using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class AssignmentBlock
    {
        public DataAccessor LhsTargetData { get; }

        public SimpleExpression RhsEvalExpression { get; }

        public AssignmentBlock(DataAccessor lhsTargetData, SimpleExpression rhsEvalExpression)
        {
            LhsTargetData = lhsTargetData;
            RhsEvalExpression = rhsEvalExpression;
        }
    }
}
