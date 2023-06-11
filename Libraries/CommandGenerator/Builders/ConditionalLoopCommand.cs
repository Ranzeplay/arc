using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class ConditionalLoopCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<ConditionalLoopBlock> source)
        {
            // TODO: Experimental
            var expression = ExpressionCommand.Build(source.TransferToNewComponent(source.Component.Condition))!;
            var actionBlock = ActionBlockCommand.Build(source.TransferToNewComponent(source.Component.Actions))!;

            var result = new PartialGenerationResult();
            result.Combine(expression);
            result.Combine(actionBlock);

            result.RelocationReferences.Add(new(0, RelocationReferenceType.WhileEntrance));
            result.RelocationReferences.Add(new(result.Commands.Count, RelocationReferenceType.EndWhile));

            return result;
        }   
    }
}
