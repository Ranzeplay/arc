using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public record FunctionCallBlock : FunctionCallBase
    {
        public FunctionCallBlock(Identifier targetFunctionIdentifier, FunctionArgument[] arguments)
            : base(targetFunctionIdentifier, arguments)
        {
        }
    }
}
