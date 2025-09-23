namespace Arc.Compiler.PackageGenerator.Models.Relocation
{
    public enum ArcSymbolType
    {
        Namespace = 0x01,
        Function = 0x02,
        GroupField = 0x03,
        DataType = 0x04,
        Constant = 0x05,
        Group = 0x06,
        Annotation = 0x07,
        Enum = 0x08,
        EnumMember = 0x09,
        GenericTypeParameter = 0x0A,
        Invalid = 0x00
    }
}
