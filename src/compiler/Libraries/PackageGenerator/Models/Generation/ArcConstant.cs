using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    public class ArcConstant
    {
        public ulong Id { get; set; }

        public ulong TypeId { get; set; }

        public required bool IsArray { get; set; }

        public required object Value { get; set; }

        public required IArcConstantEncoder Encoder { get; set; }

        public IEnumerable<byte> RawData => Encoder.Encode(Value);

        public IEnumerable<byte> Encode() => [
                ..BitConverter.GetBytes(Id),
                ..BitConverter.GetBytes(TypeId),
                ..BitConverter.GetBytes(IsArray),
                ..BitConverter.GetBytes(RawData.LongCount()),
                ..RawData
            ];
    }
}
