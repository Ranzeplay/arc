using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    internal class ArcPartialGenerationResult
    {
        public IEnumerable<byte> GeneratedData { get; set; } = [];

        public IEnumerable<ArcRelocationTarget> RelocationTargets { get; set; } = [];

        public IEnumerable<ArcRelocationLabel> RelocationLabels { get; set; } = [];

        public IEnumerable<ArcDataSlot> DataSlots { get; set; } = [];

        public IEnumerable<ArcSymbolBase> OtherSymbols { get; set; } = [];

        public void Append(ArcPartialGenerationResult generationResult)
        {
            throw new NotImplementedException();
        }
    }
}
