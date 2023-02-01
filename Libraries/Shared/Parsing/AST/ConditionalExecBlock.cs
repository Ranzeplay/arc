using Arc.Compiler.Shared.Parsing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public class ConditionalExecBlock
    {
        public ConditionalBlock[] ConditionalBlocks { get; }

        public ActionBlock? OtherwiseBlock { get; }

        public ConditionalExecBlock(ConditionalBlock[] conditionalBlocks, ActionBlock? otherwiseBlock)
        {
            ConditionalBlocks = conditionalBlocks;
            OtherwiseBlock = otherwiseBlock;
        }
    }
}
