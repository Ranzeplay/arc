using Arc.Cmdec.Models;
using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec.Commands
{
    internal class CommandSelector
    {
        public static IEnumerable<DecodedCommand> ParseAllCommands(IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var result = new List<DecodedCommand>();

            var index = 0;
            while (index < commands.Count())
            {
                var command = ParseCommand(index, commands.Skip(index), metadata);
                if (command != null)
                {
                    result.Add(command.DecodedCommand);
                    index += command.Length;
                }
                else
                {
                    throw new InvalidDataException($"Invalid operation at package content index {index} from command section");
                }
            }

            return result;
        }

        public static DecodeResult? ParseCommand(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            return commands.ElementAt(0) switch
            {
                0xA1 => ObjectCommand.CreateLocal(location, commands),
                0xA2 => ObjectCommand.CreateGlobal(location, commands),
                0xA3 => ObjectCommand.DeleteLocal(location, commands, metadata),
                0xA4 => ObjectCommand.DeleteGlobal(location, commands, metadata),
                0xC1 => JumpCommand.Relative(location, commands, metadata),
                0xC2 => JumpCommand.Conditional(location, commands, metadata),
                0xD1 => FunctionCommand.Enter(location, commands, metadata),
                0xD2 => FunctionCommand.LeaveWithoutValue(location, commands),
                0xD3 => FunctionCommand.LeaveWithValue(location, commands),
                0xE1 => MathCommand.SelectCalculation(location, commands),
                0xE2 => MathCommand.SelectLogical(location, commands),
                0xE3 => MathCommand.SelectRelation(location, commands),
                _ => null,
            };
        }
    }
}
