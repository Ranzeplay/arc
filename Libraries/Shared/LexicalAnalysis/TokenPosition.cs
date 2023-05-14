using Arc.Compiler.Shared.Compilation;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public record TokenPosition(SourceFile SourceFile, int Position, int Length)
    {
        public string RawText
        {
            get
            {
                return SourceFile.Content.Substring(Position, Length);
            }
        }
    }
}
