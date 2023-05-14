using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    internal class ConditionalLoopBlockBuilder
    {
        public static SectionBuildResult<ConditionalLoopBlock>? Build(ExpressionBuildModel model)
        {
            return ConditionalBlockBuilder.Build(KeywordToken.While, model);
        }
    }
}
