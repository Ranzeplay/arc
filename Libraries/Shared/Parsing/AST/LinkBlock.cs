using Arc.Compiler.Shared.Parsing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
