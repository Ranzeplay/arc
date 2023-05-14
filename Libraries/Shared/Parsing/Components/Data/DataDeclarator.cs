namespace Arc.Compiler.Shared.Parsing.Components.Data
{
    public record DataDeclarator(DataType DataType, Identifier Identifier, bool IsConstant)
    {
    }
}
