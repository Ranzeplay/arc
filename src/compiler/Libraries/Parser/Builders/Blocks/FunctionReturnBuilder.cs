using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    internal class FunctionReturnBuilder
    {
        public static SectionBuildResult<FunctionReturnBlock>? Build(ExpressionBuildModel model)
        {
            if (model.Tokens[0].GetKeyword().GetValueOrDefault() != KeywordToken.Return)
            {
                return null;
            }

            // This is a statement
            var semicolonIndex = Utils.GetNextSemicolonPos(model.Tokens);
            if (semicolonIndex == 1)
            {
                // Return without value
                return new(new FunctionReturnBlock(), 2);
            }
            else
            {
                // Return with value
                var expressionResult = ExpressionBuilder.Build(new(model.Tokens[1..semicolonIndex], model.DeclaredData, model.DeclaredFunctions));
                if (expressionResult != null)
                {
                    return new(new FunctionReturnBlock(expressionResult.Section), semicolonIndex + 1);
                }
            }

            return null;
        }
    }
}
