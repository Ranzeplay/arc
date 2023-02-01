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
    public class ConditionalExecBlockBuilder
    {
        public static SectionBuildResult<ConditionalExecBlock>? Build(ExpressionBuildModel model)
        {
            var conditionalBlocks = new List<ConditionalBlock>();

            // Build IfBlock
            var rootBlock = ConditionalBlockBuilder.Build(KeywordToken.If, model);
            if (rootBlock != null)
            {
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

            return null;
        }

        private static SectionBuildResult<ActionBlock>? BuildOtherwiseBlock(ExpressionBuildModel model)
        {
            var leadingKeyword = model.Tokens[0].GetKeyword();
            if (leadingKeyword != null)
            {
                if (leadingKeyword == KeywordToken.Else)
                {
                    var actionBlockPair = model.Tokens[1].GetContainer().GetValueOrDefault();
                    if (actionBlockPair == ContainerToken.Brace)
                    {
                        var actionBlockZone = Utils.PairContainer(model.Tokens[1..]);
                        if (actionBlockZone != null)
                        {
                            actionBlockZone = actionBlockZone[1..^1];
                            var block = ActionBlockBuilder.Build(new(actionBlockZone, model.DeclaredData, model.DeclaredFunctions));

                            if (block != null)
                            {
                                return new(block.Section, block.Length);
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
