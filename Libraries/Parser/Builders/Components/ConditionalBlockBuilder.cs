using Arc.Compiler.Parser.Builders.Blocks;
using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Components
{
    public class ConditionalBlockBuilder
    {
        public static SectionBuildResult<ConditionalLoopBlock>? Build(KeywordToken leadingKeywordType, ExpressionBuildModel model)
        {
            var leadingKeyword = model.Tokens[0].GetKeyword();
            if (leadingKeyword != null)
            {
                if (leadingKeyword == leadingKeywordType)
                {
                    // Build condition
                    var pair = model.Tokens[1].GetContainer().GetValueOrDefault();
                    if (pair == ContainerToken.Bracket)
                    {
                        var exprZone = Utils.PairContainer(model.Tokens[1..]);
                        if (exprZone != null)
                        {
                            // Remove bracket on the side
                            exprZone = exprZone[1..^1];
                            // Build expression
                            var expr = ExpressionBuilder.BuildRelationalExpression(new(exprZone, model.DeclaredData, model.DeclaredFunctions));
                            if (expr != null)
                            {
                                // exprZone was deleted 2 tokens (pair container)
                                int actionBlockBeginIndex = 1 + (exprZone.Length + 2);
                                var actionBlockPair = model.Tokens[actionBlockBeginIndex].GetContainer().GetValueOrDefault();
                                if (actionBlockPair == ContainerToken.Brace)
                                {
                                    var actionBlockZone = Utils.PairContainer(model.Tokens[actionBlockBeginIndex..]);
                                    if (actionBlockZone != null)
                                    {
                                        actionBlockZone = actionBlockZone[1..^1];
                                        var block = ActionBlockBuilder.Build(new(actionBlockZone, model.DeclaredData, model.DeclaredFunctions));

                                        if (block != null)
                                        {
                                            return new(new(expr.Section, block.Section), 1 + (actionBlockZone.Length + 2) + (exprZone.Length + 2));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
