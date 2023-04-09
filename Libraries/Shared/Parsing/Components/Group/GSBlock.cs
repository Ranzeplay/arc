using Arc.Compiler.Shared.Parsing.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public record GSBlock(bool IsExist, ActionBlock? Actions)
    {
    }
}
