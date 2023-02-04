using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Builders.Components.Data;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    public class FunctionBlockBuilder
    {
        public static SectionBuildResult<FunctionBlock>? Build(ExpressionBuildModel model)
        {
            // Validate leading tokens
            if (!(model.Tokens[0].GetKeyword().GetValueOrDefault() == KeywordToken.Declare && model.Tokens[1].GetKeyword().GetValueOrDefault() == KeywordToken.Func))
            {
                return null;
            }

            var result = FunctionBlockBaseBuilder.Build(model.SkipTokens(2));
            if (result != null)
            {
                return new(result.Section, result.Length + 2);
            }

            return null;
        }
    }
}
