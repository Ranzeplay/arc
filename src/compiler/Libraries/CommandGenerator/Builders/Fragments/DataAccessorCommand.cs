using Arc.Compiler.CommandGenerator.Builders;
using Arc.Compiler.CommandGenerator.Models;
using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.Compiler.CommandGenerator.Builders.Fragments
{
    internal class DataAccessorCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<DataAccessor> source)
        {
            var commands = new List<byte>();

            var accessorGenerationSource = source.TransferToNewComponent(source.Component);
            var targetDataAccessor = new DataAccessorSource(accessorGenerationSource);

            if (targetDataAccessor.DataAccessor.AccessorType == DataAccessorType.ArrayElement)
            {
                // Evaluate index first
                var indexExpression = ExpressionCommand.Build(source.TransferToNewComponent(targetDataAccessor.DataAccessor.IndexEvalExpression!));
                if (indexExpression != null)
                {
                    commands.AddRange(indexExpression.Commands);
                }
            }

            switch (targetDataAccessor.Origin)
            {
                case DataAccessorSource.AccessorOrigin.Local:
                    commands.Add(0x00);
                    break;
                case DataAccessorSource.AccessorOrigin.Global:
                    commands.Add(0x01);
                    break;
            }

            var slot = source.PackageMetadata.GenerateSlotData(targetDataAccessor.Slot);
            commands.AddRange(slot);

            return new(commands);
        }
    }
}
