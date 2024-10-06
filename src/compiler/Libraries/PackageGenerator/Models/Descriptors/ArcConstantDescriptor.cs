using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcConstantDescriptor : ArcSymbolBase
    {
        public ArcDataTypeDescriptor DataType { get; set; }

        public IEnumerable<byte> Bytes { get; set; }
    }
}
