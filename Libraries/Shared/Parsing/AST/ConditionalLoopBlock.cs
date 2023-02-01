using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class ConditionalLoopBlock : ConditionalBlock
    {
        public ConditionalLoopBlock(RelationalExpression condition, ActionBlock actionBlock)
            : base(condition, actionBlock)
        {
        }
    }
}
