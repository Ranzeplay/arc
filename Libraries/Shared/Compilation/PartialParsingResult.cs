using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Compilation
{
    public class PartialParsingResult
    {
        public List<LinkBlock> Links { get; set; } = new();

        public List<GroupBlock> DeclaredGroups { get; set; } = new();

        public List<FunctionBlock> DeclaredFunctions { get; set; } = new();

        public PartialParsingResult(List<LinkBlock> links, List<GroupBlock> declaredGroups, List<FunctionBlock> declaredFunctions)
        {
            Links = links;
            DeclaredGroups = declaredGroups;
            DeclaredFunctions = declaredFunctions;
        }
    }
}
