using Arc.Compiler.Shared.Parsing.Components.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class FunctionReturnBlock
    {
        public SimpleExpression? Expression { get; }

        public FunctionReturnBlock(SimpleExpression? expression = null)
        {
            Expression = expression;
        }
    }
}
