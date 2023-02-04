using Arc.Compiler.Shared.Parsing.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public class GSBlock
    {
        public bool IsExist { get; }

        public ActionBlock? ActionBlock { get; }

        public GSBlock(bool isExist, ActionBlock? actionBlock = null)
        {
            IsExist = isExist;
            ActionBlock = actionBlock;
        }
    }
}
