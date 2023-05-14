using System.ComponentModel;

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
