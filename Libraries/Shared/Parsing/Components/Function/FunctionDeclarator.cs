using Arc.Compiler.Shared.Parsing.Components.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public record FunctionDeclarator(Identifier Identifier, DataType ReturnDataType, FunctionParameter[] Parameters)
    {
    }
}
