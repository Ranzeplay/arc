using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    internal class LoopBlockBuilder
    {
        public static SectionBuildResult<LoopBlock>? Build(ExpressionBuildModel model)
        {
            if (model.Tokens[0].GetKeyword().GetValueOrDefault() != KeywordToken.Loop)
            {
                return null;
            }

            if (model.Tokens[1].GetContainer().GetValueOrDefault() != ContainerToken.Brace)
            {
                return null;
            }

            var actionBlockZone = Utils.PairContainer(model.Tokens[1..]);
            if (actionBlockZone == null)
            {
                return null;
            }

            // Remove pair container
            var actionBlock = ActionBlockBuilder.Build(new(actionBlockZone[1..^1], model.DeclaredData, model.DeclaredFunctions));
            if (actionBlock == null)
            {
                return null;
            }

            if (model.Tokens[actionBlockZone.Length].GetContainer().GetValueOrDefault() != ContainerToken.AntiBrace)
            {
                return null;
            }

            return new(new(actionBlock.Section), 2 + actionBlockZone.Length);
        }
    }
}
