namespace Arc.Compiler.SyntaxAnalyzer.Models.Function;

public enum ArcGroupLifecycleStageType
{
    Construction = 0x01,
    Destruction = 0x02,
    DeepCopy = 0x03,
    ShallowCopy = 0x04,
    Invalid = 0x00
}
