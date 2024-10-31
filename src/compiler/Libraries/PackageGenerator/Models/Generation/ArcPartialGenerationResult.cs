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

        public IEnumerable<ArcConstant> AddedConstants { get; set; } = [];

        public void Append(ArcPartialGenerationResult generationResult)
        {
            RelocationTargets = RelocationTargets.Concat(generationResult.RelocationTargets.Select(r =>
            {
                if (r.TargetType == ArcRelocationTargetType.Absolute)
                {
                    r.Location += GeneratedData.LongCount();
                }

                return r;
            }));
            RelocationLabels = RelocationLabels.Concat(generationResult.RelocationLabels.Select(l =>
            {
                l.Location += GeneratedData.LongCount();
                return l;
            }));
            DataSlots = DataSlots.Concat(generationResult.DataSlots);
            OtherSymbols = OtherSymbols.Concat(generationResult.OtherSymbols);

            GeneratedData = GeneratedData.Concat(generationResult.GeneratedData);

            AddedConstants = AddedConstants.Concat(generationResult.AddedConstants);
        }
    }
}
