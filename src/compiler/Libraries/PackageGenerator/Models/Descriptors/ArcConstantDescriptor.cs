using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcConstantDescriptor : ArcSymbolBase
    {
        public ArcDataDeclarationDescriptor DataType { get; set; }

        public IEnumerable<byte> Bytes { get; set; }
    }
}
