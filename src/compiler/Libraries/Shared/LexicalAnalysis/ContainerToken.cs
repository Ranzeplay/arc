namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public enum ContainerToken
    {
        Invalid,
        Brace,       // {
        AntiBrace,   // }
        Bracket,     // (
        AntiBracket, // )
        Index,       // [
        AntiIndex    // ]
    }
}
