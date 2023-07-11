using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class DeclarationCommand
    {
        public static PartialGenerationResult Build(GenerationContext<DataDeclarationBlock> source)
        {
            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Object, (byte)ObjectCommand.CreateLocal).ToList();

            commands.Add((byte)(source.Component.DataType.IsArray ? 0x01 : 0x00));

            return new(commands, Enumerable.Empty<DataDeclarator>().Append(source.Component));
        }
    }
}
