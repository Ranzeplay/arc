namespace Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Symbol;

public enum ArcSymbolType
{
    Invalid = 0x00,
    Namespace = 0x01,
    Function = 0x02,
    Group = 0x03,
    GroupField = 0x04,
    GroupFunction = 0x05,
    DataType = 0x06,
    Annotation = 0x07,
    Constant = 0x08,
}