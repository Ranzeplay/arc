using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public record GroupFunction : FunctionBlock
    {
        public GroupFunction(FunctionDeclarator declarator, ActionBlock actions)
            : base(declarator, actions)
        {
        }
    }
}
