using Arc.Compiler.PackageGenerator.Models.Descriptors;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ArcGenerationSource<T>
    {
        public T Value { get; set; }

        public Dictionary<long, object> Symbols { get; set; } = [];

        public ArcPackageDescriptor PackageDescriptor { get; set; }

        public IEnumerable<ArcDataSlot> DataSlots { get; set; }

        public void Merge(ArcGenerationResult result)
        {
            foreach (var o in result.Symbols)
            {
                if (!Symbols.ContainsKey(o.Key))
                {
                    Symbols.Add(o.Key, o.Value);
                }
            }

            DataSlots = Symbols.Values.Where(x => x is ArcDataSlot).Select(x => x as ArcDataSlot);
        }

        public ArcGenerationSource<T> Migrate<T>(T value)
        {
            return new ArcGenerationSource<T>
            {
                Value = value,
                Symbols = Symbols,
                PackageDescriptor = PackageDescriptor,
                DataSlots = DataSlots
            };
        }
    }
}
