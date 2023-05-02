using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CommandGeneration.Mappings
{
    public enum StackCommand
    {
        Invalid = 0x0,
        PushInstant = 0x1,
        PushFromObject = 0x2,
        PushFromConstant = 0x3,
        Pop = 0x4,
        PopToObject = 0x5,
    }
}
