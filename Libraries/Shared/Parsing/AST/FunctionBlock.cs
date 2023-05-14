using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public record FunctionBlock(FunctionDeclarator Declarator, ActionBlock Actions)
    {
    }
}
