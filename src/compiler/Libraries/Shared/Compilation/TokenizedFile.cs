using Arc.Compiler.Shared.LexicalAnalysis;

namespace Arc.Compiler.Shared.Compilation
{
    public record TokenizedFile(SourceFile SourceFile, Token[] Tokens)
    {
    }
}
