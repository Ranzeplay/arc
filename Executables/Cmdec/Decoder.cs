using Arc.Cmdec.Commands;
using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec
{
    internal class Decoder
    {
        public static DecodedPackage Decode(IEnumerable<byte> commands)
        {
            var metadata = GetMetadata(commands);
            var parsedCommands = ParseCommands(commands.Skip(6), metadata);

            return new(metadata, parsedCommands);
        }

        public static PackageMetadata GetMetadata(IEnumerable<byte> commands)
        {
            return new(commands.ElementAt(0),
                       commands.ElementAt(1),
                       commands.ElementAt(2),
                       commands.ElementAt(3),
                       commands.ElementAt(4),
                       commands.ElementAt(5));
        }

        public static IEnumerable<DecodedCommand> ParseCommands(IEnumerable<byte> commands, PackageMetadata metadata)
        {
            return CommandSelector.ParseAllCommands(commands, metadata);
        }
    }
}
