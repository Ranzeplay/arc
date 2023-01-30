using Arc.Compiler.Parser.Builders.Components.Data;
using Arc.Compiler.Parser.Builders.Components.Expression;
using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    public class DataAssignmentBuilder
    {
        public static SectionBuildResult<DataAssignmentBlock>? Build(ExpressionBuildModel model)
        {
            var accessor = DataAccessorBuilder.Build(model);

            if (accessor != null)
            {
                // Check assignment operator
                var opToken = model.Tokens[accessor.Length];
                if (opToken.TokenType == Shared.LexicalAnalysis.TokenType.Operator)
                {
                    var op = opToken.GetOperator();
                    if (op != null)
                    {
                        if (op.Type == Shared.LexicalAnalysis.OperatorTokenType.Assignment)
                        {
                            // Build expression after that
                            var nextSemicolonPos = Utils.GetNextSemicolonPos(model.Tokens);
                            var expr = ExpressionBuilder.BuildSimpleExpression(new(model.Tokens[(accessor.Length + 1)..nextSemicolonPos], model.DeclaredData, model.DeclaredFunctions));

                            if (expr != null)
                            {
                                return new(new(accessor.Section, expr.Section), nextSemicolonPos + 1);
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
