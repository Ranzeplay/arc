using Arc.Compiler.Shared.Parsing.Components;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class LinkBlock
    {
        public LinkTargetType TargetType { get; }

        public Identifier? Scope { get; }
        public string? Path { get; }

        public LinkBlock(Identifier scope)
        {
            TargetType = LinkTargetType.Scope;
            Scope = scope;
        }

        public LinkBlock(string path)
        {
            TargetType = LinkTargetType.Path;
            Path = path;
        }
    }
}
