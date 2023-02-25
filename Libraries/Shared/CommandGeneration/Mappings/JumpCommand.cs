using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CommandGeneration
{
    public enum JumpCommand
    {
        Invalid = 0x0,
        ToRelative = 0x1,
        Conditioal = 0x2,
    }
}
