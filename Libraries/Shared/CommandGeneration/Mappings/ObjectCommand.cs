namespace Arc.Compiler.Shared.CommandGeneration.Mappings
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
