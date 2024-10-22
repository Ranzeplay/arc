using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal class ArcBaseType : ArcTypeBase
    {
        public byte[] FieldId => [0x00];
    }
}
