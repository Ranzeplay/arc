using Arc.Compiler.Shared.Parsing.Components.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public record FunctionParameter : DataDeclarator
    {
        public FunctionParameter(DataType dataType, Identifier identifier, bool isConstant)
            : base(dataType, identifier, isConstant)
        {
        }
    }
}
