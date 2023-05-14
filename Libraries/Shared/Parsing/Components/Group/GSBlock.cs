using Arc.Compiler.Shared.Parsing.AST;

namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public record GSBlock(bool IsExist, ActionBlock? Actions)
    {
    }
}
