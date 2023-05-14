using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class AssignmentCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<DataAssignmentBlock> source)
        {
            // Build evaluate expression on rhs
            var result = ExpressionCommand.BuildSimpleExpression(source.TransferToNewComponent(source.Component.RhsEvalExpression))!;

            // Build data accessor command
            var accessorGenerationSource = source.TransferToNewComponent(source.Component.LhsTargetData);
            var targetDataAccessor = new DataAccessorSource(accessorGenerationSource);

            if (targetDataAccessor.DataAccessor.AccessorType == DataAccessorType.ArrayElement)
            {
                // Evaluate index first if exists
                var indexExpression = ExpressionCommand.BuildSimpleExpression(source.TransferToNewComponent(source.Component.LhsTargetData.IndexEvalExpression!));
                if (indexExpression != null)
                {
                    result.Combine(indexExpression);
                }
            }

            // Push leading command
            var leadingCommands = Utils.CombineLeadingCommand((byte)RootCommand.Stack, (byte)StackCommand.PopToObject);
            result.Commands.AddRange(leadingCommands);

            // Add zone descriptor
            switch (targetDataAccessor.Origin)
            {
                case DataAccessorSource.AccessorOrigin.Local:
                    result.Commands.Add(0x00);
                    break;
                case DataAccessorSource.AccessorOrigin.Global:
                    result.Commands.Add(0x01);
                    break;
            }

            var slot = source.PackageMetadata.GenerateSlotData(targetDataAccessor.Slot);
            result.Commands.AddRange(slot);

            return result;
        }
    }
}
