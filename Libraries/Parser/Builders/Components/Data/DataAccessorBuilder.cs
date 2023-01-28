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
    public class DataAccessorBuilder
    {
        public static SectionBuildResult<DataAccessor>? Build(ExpressionBuildModel model)
        {
            var identifierResult = IdentifierBuilder.Build(model.Tokens);
            if (identifierResult is not null)
            {
                var identifier = identifierResult.Section;
                var targetDeclarator = model.DeclaredData.SingleOrDefault(d => d.Identifier.Equals(identifier));
                if (targetDeclarator is not null)
                {
                    /* Check if this an ArrayElement accessor
                     * Format: ident[expr]
                     *              ^
                     *        Index container
                     */
                    var arrayElementExpressionBeginIndex = identifierResult.Length;
                    if (model.Tokens.Length > arrayElementExpressionBeginIndex)
                    {
                        if (model.Tokens[arrayElementExpressionBeginIndex].GetContainer().GetValueOrDefault() == ContainerToken.Index)
                        {
                            // Build expression
                            var expressionResult = ExpressionBuilder.BuildSimpleExpression(model.SkipTokens(arrayElementExpressionBeginIndex + 1));
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

                    // This is a singleton accessor
                    return new(new(targetDeclarator), identifierResult.Length);
                }
            }

            return null;
        }
    }
}
