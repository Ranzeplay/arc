using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class ConditionalExecCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<ConditionalExecBlock> source)
        {
            var result = new PartialGenerationResult();

            var remainingBlocks = source.Component.ConditionalBlocks.Length + (source.Component.OtherwiseBlock == null ? 0 : 1);

            // Build first conditional block
            {
                var block = source.Component.ConditionalBlocks.FirstOrDefault() ??
                    throw new InvalidOperationException("Impossible that a ConditionalExecBlock doesn't have the first condition block");

                var current = new PartialGenerationResult();

                var beginRef = new RelocationReference(current.Commands.Count, RelocationReferenceType.IfEntrance);
                current.RelocationReferences.Add(beginRef);

                var evalExpr = ExpressionCommand.Build(source.TransferToNewComponent(block.Condition))!;
                current.Combine(evalExpr);

                var condition = JumpCommand.BuildConditional(source.TransferToNewComponent(
                    new JumpRelativeCommandViewModel(
                        new RelativeRelocator(RelativeRelocatorType.Address, source.PackageMetadata.ConditionalJumpCommandLength()),
                        new RelativeRelocator(RelativeRelocatorType.IgnoreActionBlock, remainingBlocks)
                        )
                    ));
                current.Combine(condition);

                var actions = ActionBlockCommand.Build(source.TransferToNewComponent(block.Actions))!;
                current.Combine(actions);

                var jumpOutCommand = JumpCommand.BuildRelative(source.TransferToNewComponent(new RelativeRelocator(RelativeRelocatorType.IgnoreActionBlock, remainingBlocks - 1)));
                current.Combine(jumpOutCommand);

                var endRef = new RelocationReference(current.Commands.Count, RelocationReferenceType.EndIf);
                current.RelocationReferences.Add(endRef);

                result.Combine(current);
                remainingBlocks -= 1;
            }

            // Build each conditional blocks
            foreach (var block in source.Component.ConditionalBlocks.Skip(1))
            {
                var current = new PartialGenerationResult();

                var beginRef = new RelocationReference(current.Commands.Count, RelocationReferenceType.ElifEntrance);
                current.RelocationReferences.Add(beginRef);

                var evalExpr = ExpressionCommand.Build(source.TransferToNewComponent(block.Condition))!;
                current.Combine(evalExpr);

                var condition = JumpCommand.BuildConditional(source.TransferToNewComponent(
                    new JumpRelativeCommandViewModel(
                        new RelativeRelocator(RelativeRelocatorType.Address, source.PackageMetadata.ConditionalJumpCommandLength()),
                        new RelativeRelocator(RelativeRelocatorType.IgnoreActionBlock, remainingBlocks)
                        )
                    ));
                current.Combine(condition);

                var actions = ActionBlockCommand.Build(source.TransferToNewComponent(block.Actions))!;
                current.Combine(actions);

                var jumpOutCommand = JumpCommand.BuildRelative(source.TransferToNewComponent(new RelativeRelocator(RelativeRelocatorType.IgnoreActionBlock, remainingBlocks - 1)));
                current.Combine(jumpOutCommand);

                var endRef = new RelocationReference(current.Commands.Count, RelocationReferenceType.EndElif);
                current.RelocationReferences.Add(endRef);

                result.Combine(current);
                remainingBlocks -= 1;
            }

            // Check ElseBlock, build if exists
            if (remainingBlocks > 0)
            {
                var block = source.Component.OtherwiseBlock!;

                var current = new PartialGenerationResult();

                var beginRef = new RelocationReference(current.Commands.Count, RelocationReferenceType.ElseEntrance);
                current.RelocationReferences.Add(beginRef);

                var actions = ActionBlockCommand.Build(source.TransferToNewComponent(block))!;
                current.Combine(actions);

                var endRef = new RelocationReference(current.Commands.Count, RelocationReferenceType.EndElse);
                current.RelocationReferences.Add(endRef);

                result.Combine(current);
            }

            return result;
        }
    }
}
