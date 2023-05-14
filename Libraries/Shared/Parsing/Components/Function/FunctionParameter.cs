using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public record FunctionParameter : DataDeclarator
    {
        public FunctionParameter(DataType dataType, Identifier identifier, bool isConstant)
            : base(dataType, identifier, isConstant)
        {
        }
    }
}
