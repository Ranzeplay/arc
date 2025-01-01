using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    public class ArcPartialGenerationResult
    {
        public List<byte> GeneratedData { get; init; } = [];

        public List<ArcRelocationTarget> RelocationTargets { get; init; } = [];

        public List<ArcRelocationLabel> RelocationLabels { get; init; } = [];

        public List<ArcDataSlot> DataSlots { get; set; } = [];

        public List<ArcSymbolBase> OtherSymbols { get; } = [];

        public List<ArcConstant> AddedConstants { get; } = [];

        public void Append(ArcPartialGenerationResult generationResult)
        {
            RelocationTargets.AddRange(generationResult.RelocationTargets.Select(r =>
            {
                if (r.TargetType == ArcRelocationTargetType.Absolute)
                {
                    r.TargetLocation += GeneratedData.Count;
                }

                r.Location += GeneratedData.Count;

                return r;
            }).ToList());
            RelocationLabels.AddRange(generationResult.RelocationLabels.Select(l =>
            {
                l.Location += GeneratedData.Count;
                return l;
            }).ToList());
            DataSlots.AddRange(generationResult.DataSlots);
            OtherSymbols.AddRange(generationResult.OtherSymbols);

            GeneratedData.AddRange(generationResult.GeneratedData);

            AddedConstants.AddRange(generationResult.AddedConstants);
        }
    }
}
