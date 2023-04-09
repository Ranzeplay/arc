using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Compilation
{
    public record PartialParsingResult(List<LinkBlock> Links, List<GroupBlock> DeclaredGroups, List<FunctionBlock> DeclaredFunctions)
    {
    }
}
