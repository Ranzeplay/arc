using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public record GroupBlock(Identifier Identifier, GroupField[] Fields, GroupMethod[] GroupMethods, GroupFunction[] Functions)
    {
    }
}
