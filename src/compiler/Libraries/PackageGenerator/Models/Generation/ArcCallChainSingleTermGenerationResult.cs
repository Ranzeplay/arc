using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    internal class ArcCallChainSingleTermGenerationResult
    {
        public IArcDataTypeProxy ResultDataType { get; }

        public bool IsMutable { get; }

        public bool IsArray { get; }

        public bool HasNextTerm { get; }
    }
}
