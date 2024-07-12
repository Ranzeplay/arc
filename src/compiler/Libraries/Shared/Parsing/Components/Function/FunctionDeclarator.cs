using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public record FunctionDeclarator(Identifier Identifier, DataType ReturnDataType, FunctionParameter[] Parameters)
    {
    }
}
