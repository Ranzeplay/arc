using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CompilerException
{
    public enum ExceptionLevel
    {
        [Description("U")]
        Unexpected = 0,

        [Description("I")]
        Information,

        [Description ("W")]
        Warning,

        [Description("E")]
        Error,

        [Description("F")]
        Fatal
    }
}
