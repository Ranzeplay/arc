using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CompilerException
{
    public enum ExceptionZone
    {
        [Description("U")]
        Unknown = 0,

        [Description("L")]
        LexicalAnalysis,

        [Description("P")]
        Parsing,

        [Description("C")]
        CommandGeneration
    }
}
