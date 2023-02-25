using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CommandGeneration
{
    public enum ObjectCommand
    {
        Invalid = 0x0,
        CreateLocal = 0x1,
        CreateGlobal = 0x2,
        DeleteLocal = 0x3,
        DeleteGlobal = 0x4,
    }
}
