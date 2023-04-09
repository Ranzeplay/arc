using Arc.Compiler.Shared.Parsing.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.AST
{
    public record ConditionalExecBlock(ConditionalBlock[] ConditionalBlocks, ActionBlock? OtherwiseBlock)
    {
    }
}
