using System.ComponentModel;

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
