namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

public sealed class ArcGinkgoSourceInformation
{
    public string InstructionSet { get; } = "arc-ginkgo";
    public Dictionary<uint, string> SymbolMapping { get; } = [];
    public Dictionary<uint, string> FunctionSources { get; } = [];
}
