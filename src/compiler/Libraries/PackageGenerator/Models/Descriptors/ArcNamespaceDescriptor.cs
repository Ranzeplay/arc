using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcNamespaceDescriptor : ArcSymbolBase
    {
        public string Namespace { get => Name; set => Name = value; }
    }
}
