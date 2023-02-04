using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Models
{
    internal class GSBlockBuildResult
    {
        public KeywordToken GSType { get; set; }

        public GSBlock Block { get; set; }

        public GSBlockBuildResult(KeywordToken gsType, GSBlock block)
        {
            GSType = gsType;
            Block = block;
        }
    }
}
