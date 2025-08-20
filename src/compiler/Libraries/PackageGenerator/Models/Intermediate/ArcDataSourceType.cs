namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal enum ArcDataSourceType
    {
        ConstantTable = 0x01,
        DataSlot = 0x02,
        Field = 0x03,
        ArrayElement = 0x04,
        StackTop = 0x05,
        Symbol = 0x06,
        Invalid = 0x00
    }
}
