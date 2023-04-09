using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components
{
    public record ConditionalBlock(RelationalExpression Condition, ActionBlock Actions)
    {
    }
}
