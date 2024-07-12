using Arc.Compiler.Shared.Parsing.Components;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public record ConditionalExecBlock(ConditionalBlock[] ConditionalBlocks, ActionBlock? OtherwiseBlock)
    {
    }
}
