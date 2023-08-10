using Arc.Compiler.Parser.Models;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Utils;

namespace Arc.Compiler.Parser.Builders.Blocks
{
    public class ActionBlockBuilder
    {
        internal static SectionBuildResult<ActionBlock>? Build(ExpressionBuildModel model)
        {
            var result = new List<ASTNode>();

            // Try to build each one every iteration
            int index = 0;
            while (index < model.Tokens.Length)
            {
                var assignment = DataAssignmentBuilder.Build(model.SkipTokens(index));
                if (assignment != null)
                {
                    result.Add(new(assignment.Section));
                    index += assignment.Length;
                    continue;
                }

                var declaration = DataDeclarationBuilder.Build(model.SkipTokens(index).Tokens);
                if (declaration != null)
                {
                    result.Add(new(declaration.Section));
                    index += declaration.Length;
                    continue;
                }

                var functionCall = FunctionCallBuilder.Build(model.SkipTokens(index));
                if (functionCall != null)
                {
                    result.Add(new(functionCall.Section));
                    index += functionCall.Length;
                    continue;
                }

                var functionReturn = FunctionReturnBuilder.Build(model.SkipTokens(index));
                if (functionReturn != null)
                {
                    result.Add(new(functionReturn.Section));
                    index += functionReturn.Length;
                    continue;
                }

                var conditionalLoop = ConditionalLoopBlockBuilder.Build(model.SkipTokens(index));
                if (conditionalLoop != null)
                {
                    result.Add(new(conditionalLoop.Section));
                    index += conditionalLoop.Length;
                    continue;
                }

                var conditionalExec = ConditionalExecBlockBuilder.Build(model.SkipTokens(index));
                if (conditionalExec != null)
                {
                    result.Add(new(conditionalExec.Section));
                    index += conditionalExec.Length;
                    continue;
                }

                var loopBlock = LoopBlockBuilder.Build(model.SkipTokens(index));
                if (loopBlock != null)
                {
                    result.Add(new(loopBlock.Section));
                    index += loopBlock.Length;
                    continue;
                }

                throw new Exception($"Invalid Action type at index {index}");
            }

            return new(new(result.ToArray()), model.Tokens.Length);
        }
    }
}
