using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Group;

namespace Arc.Compiler.Shared.Compilation
{
    public record PartialParsingResult(List<LinkBlock> Links, List<GroupBlock> DeclaredGroups, List<FunctionBlock> DeclaredFunctions)
    {
    }
}
