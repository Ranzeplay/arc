using Arc.Compiler.CommandGenerator.Models;
using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.Parsing.AST;

namespace Arc.Compiler.CommandGenerator.Builders
{
    internal class FunctionReturnCommand
    {
        public static PartialGenerationResult Build(GenerationContext<FunctionReturnBlock> source)
        {
            if (source.Component.Expression != null)
            {
                var expr = ExpressionCommand.Build(source.TransferToNewComponent(source.Component.Expression));

                var commands = Utils.CombineLeadingCommand((byte)RootCommand.Function, (byte)FunctionCommand.LeaveWithValue)
                    .ToList();

                var result = expr;
                result.Commands.AddRange(commands);

                return result;
            }
            else
            {
                var commands = Utils.CombineLeadingCommand((byte)RootCommand.Function, (byte)FunctionCommand.LeaveWithoutValue)
                    .ToList();
                return new(commands);
            }
        }
    }
}
