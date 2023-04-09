using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Models
{
    internal record GSBlockBuildResult(KeywordToken GSType, GSBlock Block)
    {
    }
}
