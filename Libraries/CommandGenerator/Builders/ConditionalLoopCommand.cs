using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class ConditionalLoopCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<ConditionalLoopBlock> source)
        {
            var result = new PartialGenerationResult();

            // TODO: Experimental
            var expression = ExpressionCommand.Build(source.TransferToNewComponent(source.Component.Condition))!;

            // Conditional jump command
            var conditionalJumpCommand = new PartialGenerationResult();
            conditionalJumpCommand.Commands.AddRange(Utils.CombineLeadingCommand((byte)RootCommand.Jump, (byte)JumpCommand.Conditioal));
            // True condition
            conditionalJumpCommand.RelocationTargets.Add(RelocationTarget.NewRelativeLocation(0, conditionalJumpCommand.Commands.Count, new(RelativeRelocatorType.Address, source.PackageMetadata.ConditionalJumpCommandLength())));
            conditionalJumpCommand.Commands.AddRange(source.PackageMetadata.GenerateEmptyAddress());
            // False condition
            conditionalJumpCommand.RelocationTargets.Add(RelocationTarget.NewRelativeLocation(0, conditionalJumpCommand.Commands.Count, new(RelativeRelocatorType.IgnoreActionBlock, 1)));
            conditionalJumpCommand.Commands.AddRange(source.PackageMetadata.GenerateEmptyAddress());

            // The entrance contains the judgmental expression
            result.RelocationReferences.Add(new(0, RelocationReferenceType.WhileEntrance));
            var actionBlock = ActionBlockCommand.Build(source.TransferToNewComponent(source.Component.Actions))!;
            
            // Jump back to start (unconditional)
            var jumpToStartCommand = new PartialGenerationResult();
            jumpToStartCommand.Commands.AddRange(Utils.CombineLeadingCommand((byte)RootCommand.Jump, (byte)JumpCommand.ToRelative));
            conditionalJumpCommand.RelocationTargets.Add(RelocationTarget.NewRelativeLocation(0, conditionalJumpCommand.Commands.Count, new(RelativeRelocatorType.IterationEntry)));

            // Merge them all
            result.Combine(expression);
            result.Combine(actionBlock);
            result.Combine(jumpToStartCommand);

            // The end of while statement
            result.RelocationReferences.Add(new(result.Commands.Count, RelocationReferenceType.EndWhile));

            return result;
        }   
    }
}
