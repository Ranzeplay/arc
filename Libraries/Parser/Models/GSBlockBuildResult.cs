using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Group;

namespace Arc.Compiler.Parser.Models
{
    internal record GSBlockBuildResult(KeywordToken GSType, GSBlock Block)
    {
    }
}
