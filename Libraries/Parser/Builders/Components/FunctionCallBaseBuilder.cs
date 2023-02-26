using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Components
{
    internal class FunctionCallBaseBuilder
    {
        public static SectionBuildResult<FunctionCallBase>? Build(ExpressionBuildModel model)
        {
            // Build function identifier
            var identifierResult = IdentifierBuilder.Build(model.Tokens);
            if (identifierResult is null)
            {
                return null;
            }
            var identifier = identifierResult.Section;
            
            // Find target function
            // TODO: Maybe move this to package generation stage?
            var targetFunction = model.DeclaredFunctions.SingleOrDefault(d => d.Identifier.Equals(identifier));
            if (targetFunction == null)
            {
                return null;
            }

            // Build argument zone
            var currentIndex = identifierResult.Length;
            if (model.Tokens[currentIndex].GetContainer().GetValueOrDefault() != ContainerToken.Bracket)
            {
                return null;
            }

            var arguments = new List<FunctionArgument>();

            if (targetFunction.Parameters.Length == 0)
            {
                // This is a function without arguments(parameters)
                currentIndex++;
                if (model.Tokens[currentIndex].GetContainer().GetValueOrDefault() == ContainerToken.AntiBracket)
                {
                    return new(new(targetFunction.Identifier, arguments.ToArray()), currentIndex);
                }
            }
            else
            {
                // Build arguments
                var argumentZone = Utils.PairContainer(model.Tokens[currentIndex..]);
                if (argumentZone is null)
                {
                    return null;
                }

                currentIndex += argumentZone.Length;
                // Remove container on both sides
                var tokenList = argumentZone
                    .Skip(1)
                    .SkipLast(1)
                    .ToArray();
                var argumentTokenList = Utils.SplitCommaExpression(tokenList);

                // They must have the same length in order to check they are mentioning the same function
                if (argumentTokenList.Count == targetFunction.Parameters.Length)
                {
                    var len = argumentTokenList.Count;
                    // Function arguments and function parameters have the same order
                    for (int i = 0; i < len; i++)
                    {
                        var expressionResult = ExpressionBuilder.BuildSimpleExpression(new(argumentTokenList[i].ToArray(), model.DeclaredData, model.DeclaredFunctions));

                        if (expressionResult is not null)
                        {
                            var expression = expressionResult.Section;
                            arguments.Add(new(targetFunction.Parameters[i], expression));
                        }
                        else
                        {
                            // It shouldn't happen
                            throw new ArgumentException("Argument wasn't build successfully");
                        }
                    }

                    return new(new(identifier, arguments.ToArray()), currentIndex);
                }

                return null;
            }

            return null;
        }
    }
}
