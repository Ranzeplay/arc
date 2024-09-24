namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcConstantDescriptor
    {
        public long Id { get; set; }

        public string RawFullName { get; set; }

        public ArcDataTypeDescriptor DataType { get; set; }

        public IEnumerable<byte> Bytes { get; set; }
    }
}
