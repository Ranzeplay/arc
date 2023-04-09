using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Data
{
    public record DataDeclarator(DataType DataType, Identifier Identifier, bool IsConstant)
    {
    }
}
