using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CommandGeneration
{
    public enum FunctionCommand
    {
        Invalid = 0x0,
        Enter = 0x1,
        LeaveWithoutValue = 0x2,
        LeaveWithValue = 0x3,
    }
}
