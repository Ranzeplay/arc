using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.Compiler.Shared.CommandGeneration
{
    public record GeneratedConstant(DataType DataType, byte[] GeneratedBytes)
    {
    }
}
