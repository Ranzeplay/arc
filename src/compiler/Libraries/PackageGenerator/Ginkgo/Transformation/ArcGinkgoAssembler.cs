using Arc.Compiler.PackageGenerator.Models.Generation;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

/// <summary>Encodes and resolves one function. Jump addresses are function-local byte offsets.</summary>
public static class ArcGinkgoAssembler
{
    public static ArcPartialGenerationResult Assemble(ArcGinkgoInstructionGenerationResult input)
    {
        var (result, boundaries) = ArcGinkgoInstructionEncoder.Encode(input);
        if (!result.DiscardResult)
            ArcRelocationResolver.Resolve(result, boundaries);
        return result;
    }
}
