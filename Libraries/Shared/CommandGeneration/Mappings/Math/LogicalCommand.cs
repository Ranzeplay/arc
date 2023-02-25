using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CommandGeneration.Math
{
    public enum LogicalCommand
    {
        Invalid = 0x0,
        And = 0x1,
        Or = 0x2,
        Not = 0x3,
    }
}
