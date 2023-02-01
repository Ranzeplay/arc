using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    public class ConditionalLoopBlockBuilder
    {
        public static SectionBuildResult<ConditionalLoopBlock>? Build(ExpressionBuildModel model)
        {
            return ConditionalBlockBuilder.Build(KeywordToken.While, model);
        }
    }
}
