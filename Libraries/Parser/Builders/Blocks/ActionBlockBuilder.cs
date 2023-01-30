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
    public class ActionBlockBuilder
    {
        public static SectionBuildResult<ActionBlock>? Build(ExpressionBuildModel model)
        {
            var result = new List<ASTNode>();

            int index = 0;
            while(index < model.Tokens.Length)
            {
                var assignment = DataAssignmentBuilder.Build(model.SkipTokens(index));
                if(assignment != null)
                {
                    result.Add(new(assignment.Section));
                    index += assignment.Length;
                }

                var declaration = DataDeclarationBuilder.Build(model.SkipTokens(index).Tokens);
                if(declaration != null)
                {
                    result.Add(new(declaration.Section));
                    index += declaration.Length;
                }

                var functionCall = FunctionCallBuilder.Build(model.SkipTokens(index));
                if(functionCall != null)
                {
                    result.Add(new(functionCall.Section));
                    index += functionCall.Length;
                }

                var functionReturn = FunctionReturnBuilder.Build(model);
                if(functionReturn != null)
                {
                    result.Add(new(functionReturn.Section));
                    index += functionReturn.Length;
                }
            }

            return null;
        }
    }
}
