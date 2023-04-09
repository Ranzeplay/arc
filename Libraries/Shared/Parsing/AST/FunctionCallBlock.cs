using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public record FunctionCallBlock : FunctionCallBase
    {
        public FunctionCallBlock(Identifier targetFunctionIdentifier, FunctionArgument[] arguments)
            : base(targetFunctionIdentifier, arguments)
        {
        }
    }
}
