using Arc.Compiler.Shared.Parsing.Components.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class FunctionBlock
    {
        public FunctionDeclarator Declarator { get; }

        public ActionBlock Actions { get; }

        public FunctionBlock(FunctionDeclarator declarator, ActionBlock actions)
        {
            Declarator = declarator;
            Actions = actions;
        }
    }
}
