using Arc.Compiler.PackageGenerator.Base;

namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal class ArcBaseType : ArcTypeBase
    {
        public byte[] FieldId => [0x00];

        public ArcBaseType(ulong id, string identifier) : base(identifier)
        {
            Id = id;
            Name = identifier;
            Scope = new ArcSymbolScope() { Type = ArcSymbolScopeType.Language };
        }
    }
}
