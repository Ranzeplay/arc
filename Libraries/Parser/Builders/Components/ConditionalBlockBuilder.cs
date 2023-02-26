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
    internal class ConditionalBlockBuilder
    {
        public static SectionBuildResult<ConditionalLoopBlock>? Build(KeywordToken leadingKeywordType, ExpressionBuildModel model)
        {
            if (model.Tokens[0].GetKeyword().GetValueOrDefault() != leadingKeywordType)
            {
                return null;
            }

            // Build condition
            if (model.Tokens[1].GetContainer().GetValueOrDefault() != ContainerToken.Bracket)
            {
                return null;
            }

            var exprZone = Utils.PairContainer(model.Tokens[1..]);
            if (exprZone == null)
            {
                return null;
            }

            // Remove bracket on the side
            exprZone = exprZone[1..^1];
            // Build expression
            var expr = ExpressionBuilder.BuildRelationalExpression(new(exprZone, model.DeclaredData, model.DeclaredFunctions));
            if (expr == null)
            {
                return null;
            }

            // exprZone was deleted 2 tokens (pair container)
            int actionBlockBeginIndex = 1 + (exprZone.Length + 2);
            // Pre-check action block
            if (model.Tokens[actionBlockBeginIndex].GetContainer().GetValueOrDefault() != ContainerToken.Brace)
            {
                return null;
            }

            var actionBlockZone = Utils.PairContainer(model.Tokens[actionBlockBeginIndex..]);
            if (actionBlockZone == null)
            {
                return null;
            }

            // Build action block
            actionBlockZone = actionBlockZone[1..^1];
            var block = ActionBlockBuilder.Build(new(actionBlockZone, model.DeclaredData, model.DeclaredFunctions));

            if (block != null)
            {
                return (new(new(expr.Section, block.Section), 1 + (actionBlockZone.Length + 2) + (exprZone.Length + 2)));
            }
            else
            {
                return null;
            }
        }
    }
}
