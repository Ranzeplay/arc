using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    public class ArcConstant
    {
        public long Id { get; set; }

        public long TypeId { get; set; }

        public required object Value { get; set; }

        public required IArcConstantEncoder Encoder { get; set; }

        public IEnumerable<byte> RawData => Encoder.Encode(Value);

        public IEnumerable<byte> Encode() => [
                ..BitConverter.GetBytes(Id),
                ..BitConverter.GetBytes(TypeId),
                ..BitConverter.GetBytes(RawData.LongCount()),
                ..RawData
            ];
    }
}
