using Arc.Compiler.Parser.Builders.Components;
using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    internal class ConditionalExecBlockBuilder
    {
        public static SectionBuildResult<ConditionalExecBlock>? Build(ExpressionBuildModel model)
        {
            var conditionalBlocks = new List<ConditionalBlock>();

            // Build IfBlock
            var rootBlock = ConditionalBlockBuilder.Build(KeywordToken.If, model);
            if (rootBlock == null)
            {
                return null;
            }
            var builtLength = rootBlock.Length;
            conditionalBlocks.Add(rootBlock.Section);

            // Build ElseIf blocks
            while (true)
            {
                var block = ConditionalBlockBuilder.Build(KeywordToken.ElseIf, model.SkipTokens(builtLength));
                if (block != null)
                {
                    conditionalBlocks.Add(block.Section);
                    builtLength += block.Length;
                }
                else
                {
                    break;
                }
            }

            // Build ElseBlock
            var otherwiseBlock = BuildOtherwiseBlock(model.SkipTokens(builtLength));

            if (otherwiseBlock != null)
            {
                builtLength += otherwiseBlock.Length;
            }

            return new(new(conditionalBlocks.ToArray(), otherwiseBlock?.Section), builtLength);
        }

        private static SectionBuildResult<ActionBlock>? BuildOtherwiseBlock(ExpressionBuildModel model)
        {
            if (model.Tokens[0].GetKeyword().GetValueOrDefault() != KeywordToken.Else)
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
            actionBlockZone = actionBlockZone[1..^1];
            var block = ActionBlockBuilder.Build(new(actionBlockZone, model.DeclaredData, model.DeclaredFunctions));

            if (block != null)
            {
                return new(block.Section, block.Length);
            }
            else
            {
                return null;
            }
        }
    }
}
