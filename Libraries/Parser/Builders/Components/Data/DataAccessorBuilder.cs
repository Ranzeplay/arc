using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Components.Data
{
    internal class DataAccessorBuilder
    {
        public static SectionBuildResult<DataAccessor>? Build(ExpressionBuildModel model)
        {
            var identifierResult = IdentifierBuilder.Build(model.Tokens);
            if (identifierResult is null)
            {
                return null;
            }
            var identifier = identifierResult.Section;

            var targetDeclarator = model.DeclaredData.SingleOrDefault(d => d.Identifier.Equals(identifier));
            if (targetDeclarator is null)
            {
                return null;
            }

            /* Check if this an ArrayElement accessor
             * Format: ident[expr]
             *              ^
             *        Index container
             */
            var arrayElementExpressionBeginIndex = identifierResult.Length;
            if (model.Tokens.Length > arrayElementExpressionBeginIndex)
            {
                // Check if this is an array element accessor
                if (model.Tokens[arrayElementExpressionBeginIndex].GetContainer().GetValueOrDefault() == ContainerToken.Index)
                {
                    var indexExpressionZone = Utils.PairContainer(model.Tokens[arrayElementExpressionBeginIndex..]);
                    if (indexExpressionZone is not null)
                    {
                        // Tokens inside the index container can be built into an expression
                        var tokenList = indexExpressionZone
                            .Skip(1)
                            .SkipLast(1)
                            .ToArray();

                        // Build expression
                        var expressionResult = ExpressionBuilder.BuildSimpleExpression(new(tokenList, model.DeclaredData, model.DeclaredFunctions));
                        if (expressionResult is not null)
                        {
                            // Check format
                            var endIndex = arrayElementExpressionBeginIndex + expressionResult.Length - 1;
                            if (model.Tokens[endIndex].GetContainer().GetValueOrDefault() == ContainerToken.AntiIndex)
                            {
                                return new(new(targetDeclarator, expressionResult.Section), endIndex + 1);
                            }
                        }
                    }
                }
            }

            // This is a singleton accessor
            return new(new(targetDeclarator), identifierResult.Length);
        }
    }
}
