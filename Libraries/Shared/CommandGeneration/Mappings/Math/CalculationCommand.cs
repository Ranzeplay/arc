using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CommandGeneration.Math
{
    public enum CalculationCommand
    {
        Invalid = 0x0,
        Add = 0x1,
        Subtract = 0x2,
        Multiply = 0x3,
        Divide = 0x4,
        Modulo = 0x5,
        Inverse = 0x6,
    }
}
