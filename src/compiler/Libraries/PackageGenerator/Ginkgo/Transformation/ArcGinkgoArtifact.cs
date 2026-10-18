using Arc.Compiler.PackageGenerator.Models.Logging;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

/// <summary>A versioned compiler artifact, separate from legacy executable packages.</summary>
public sealed class ArcGinkgoArtifact
{
    public byte[] GeneratedData { get; internal set; } = [];
    public List<ArcCompilationLogBase> Logs { get; } = [];
    public bool DiscardResult => Logs.Any(l => l.Level >= LogLevel.Error);
    public ArcGinkgoSourceInformation SourceInformation { get; } = new();
}