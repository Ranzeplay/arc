using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public record FunctionCallBase(Identifier TargetFunctionIdentifier, FunctionArgument[] Arguments)
    {
    }
}
