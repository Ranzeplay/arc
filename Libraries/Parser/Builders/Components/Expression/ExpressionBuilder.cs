using Arc.Compiler.Parser.Builders.Components.Data;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.Compilation;
using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Expression;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Components.Expression
{
    public class ExpressionBuilder
    {
        /// <summary>
        /// Build original tokens into a simple expression
        /// </summary>
        /// <param name="model">All the tokens inside the model.Tokens array joins the operation of the construction of the expression</param>
        /// <returns>The expression if built successfully</returns>
        public static SectionBuildResult<SimpleExpression>? BuildSimpleExpression(ExpressionBuildModel model)
        {
            // Convert to Expression term;
            var index = 0;
            var terms = new List<ExpressionTerm>();
            while (index < model.Tokens.Length)
            {
                var iterationResult = BuildExpressionTerm(model.SkipTokens(index));
                if (iterationResult is not null)
                {
                    terms.Add(iterationResult.Section);
                    index += iterationResult.Length;
                }
            }

            if (terms.Count == 0)
            {
                return null;
            }
            else
            {
                return new(new(terms.ToArray()), model.Tokens.Length);
            }
        }

        private static SectionBuildResult<ExpressionTerm>? BuildExpressionTerm(ExpressionBuildModel model)
        {
            var first = model.Tokens[0];
            if (first.GetOperator() is not null)
            {
                var op = first.GetOperator();
                if (op is not null)
                {
                    return new(new(op), 1);
                }
            }
            else if (first.GetContainer() is not null)
            {
                var container = first.GetContainer().GetValueOrDefault();
                if (container == ContainerToken.Bracket)
                {
                    return new(new(ExpressionTermType.OpenPriority), 1);
                }
                else if (container == ContainerToken.AntiBracket)
                {
                    return new(new(ExpressionTermType.ClosePriority), 1);
                }
            }
            else
            {
                var data = BuildExpressionDataTerm(model);
                if (data is not null)
                {
                    return new(new(data.Section), data.Length);
                }
            }

            return null;
        }

        private static SectionBuildResult<ExpressionDataTerm>? BuildExpressionDataTerm(ExpressionBuildModel model)
        {
            var funcResult = FunctionCallBaseBuilder.Build(model);
            if (funcResult is not null)
            {
                return new(new(funcResult.Section), funcResult.Length);
            }

            // Build data accessor
            var accessorResult = DataAccessorBuilder.Build(model);
            if (accessorResult is not null)
            {
                return new(new(accessorResult.Section), accessorResult.Length);
            }

            var first = model.Tokens[0];
            if (first.GetNumber() is not null)
            {
                var number = first.GetNumber();
                if (number is not null)
                {
                    return new(new(ExpressionDataTermType.Number, number), 1);
                }
            }
            else if (first.GetString() is not null)
            {
                var str = first.GetString();
                if (str is not null)
                {
                    return new(new(ExpressionDataTermType.String, str), 1);
                }
            }

            return null;
        }
    }
}
