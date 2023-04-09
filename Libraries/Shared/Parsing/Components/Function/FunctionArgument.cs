using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public record FunctionArgument(FunctionParameter Declarator, SimpleExpression EvaluateExpression)
    {
    }
}
