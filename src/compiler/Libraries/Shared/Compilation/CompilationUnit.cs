namespace Arc.Compiler.Shared.Compilation
{
    public record CompilationUnit(List<TokenizedFile> TokenizedFiles, List<PartialParsingResult> Partials)
    {
    }
}
