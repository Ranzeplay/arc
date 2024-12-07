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
        Invalid = 0x00
    }
}
