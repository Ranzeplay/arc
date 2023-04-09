using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public record GroupMethod : FunctionBlock
    {
        public GroupMethod(FunctionDeclarator declarator, ActionBlock actions)
            : base(declarator, actions)
        {
        }
    }
}