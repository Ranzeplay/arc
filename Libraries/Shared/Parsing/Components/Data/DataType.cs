using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Data
{
    public record DataType(Identifier FullTypeIdentifier, bool IsArray)
    {
    }
}
