namespace Arc.Compiler.SyntaxAnalyzer;

public sealed class ArcSyntaxException(IReadOnlyList<string> diagnostics)
    : Exception(string.Join(Environment.NewLine, diagnostics))
{
    public IReadOnlyList<string> Diagnostics { get; } = diagnostics;
}
