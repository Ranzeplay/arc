using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public class FunctionCallBase
    {
        public Identifier TargetFunctionIdentifier { get; }

        public FunctionArgument[] Arguments { get; }

        public FunctionCallBase(Identifier targetFunctionIdentifier, FunctionArgument[] arguments)
        {
            TargetFunctionIdentifier = targetFunctionIdentifier;
            Arguments = arguments;
        }
    }
}
