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
    internal class FunctionCallBuilder
    {
        public static SectionBuildResult<FunctionCallBlock>? Build(ExpressionBuildModel model)
        {
            if (model.Tokens[0].GetKeyword().GetValueOrDefault() != KeywordToken.Call)
            {
                return null;
            }

            var funcBaseResult = FunctionCallBaseBuilder.Build(model.SkipTokens(1));
            if (funcBaseResult == null)
            {
                return null;
            }

            // This is a statement
            var semicolonIndex = Utils.GetNextSemicolonPos(model.Tokens);
            if (semicolonIndex == funcBaseResult.Length + 1)
            {
                var funcBase = funcBaseResult.Section;
                return new(new(funcBase.TargetFunctionIdentifier, funcBase.Arguments), funcBaseResult.Length + 2);
            }

            return null;
        }
    }
}
