using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class ActionBlock
    {
        public ASTNode[] ASTNodes { get; }

        public ActionBlock(ASTNode[] aSTNodes)
        {
            ASTNodes = aSTNodes;
        }
    }
}
