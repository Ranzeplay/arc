namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    public class ArcConstant
    {
        public long Id { get; set; }

        public long TypeId { get; set; }

        public object Value { get; set; }

        public IEnumerable<byte> RawData { get; set; } = [];

        public IEnumerable<byte> Encode() => BitConverter.GetBytes(TypeId).Concat(RawData);
    }
}
