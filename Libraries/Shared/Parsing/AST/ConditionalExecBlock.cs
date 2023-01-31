using Arc.Compiler.Shared.Parsing.Components.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class ConditionalExecBlock
    {
        public RelationalExpression Condition { get; }

        public ActionBlock ActionBlock { get; }

        public ConditionalExecBlock(RelationalExpression condition, ActionBlock actionBlock)
        {
            Condition = condition;
            ActionBlock = actionBlock;
        }
    }
}
