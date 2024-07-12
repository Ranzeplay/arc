using Arc.Cmdec.Commands;
using Arc.Cmdec.Models;
using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec
{
    internal class Decoder
    {
        public static DecodedPackage Decode(IEnumerable<byte> commands)
        {
            var index = 0;

            var metadata = GetMetadata(commands);
            index += 6;

            var constantTable = DecodeConstantTable(commands.Skip(index), metadata);
            index += (int)constantTable.Item2;

            var functionTable = DecodeFunctionTable(commands.Skip(index), metadata);
            index += (int)functionTable.Item2;

            var parsedCommands = ParseCommands(commands.Skip(index), metadata);

            return new(metadata, parsedCommands, functionTable.Item1, constantTable.Item1);
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

        public static (IEnumerable<DecodedConstant>, long) DecodeConstantTable(IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var count = Utils.BytesToLong(commands.Take(metadata.DataSlotAlignment).ToArray());
            int index = metadata.DataSlotAlignment;
            var result = new List<DecodedConstant>();
            for (var i = 0; i < count; i++)
            {
                var slot = Utils.BytesToLong(commands.Skip(index).Take(metadata.DataSlotAlignment).ToArray());
                index += metadata.DataSlotAlignment;
                var data = Utils.DecodeDataBlock(commands.Skip(index).ToArray(), metadata);

                result.Add(new(slot, data.Item1));
                index += data.Item2;
            }

            return (result, index);
        }

        public static (IEnumerable<DecodedFunctionEntry>, long) DecodeFunctionTable(IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var count = Utils.BytesToLong(commands.Take(metadata.AddressAlignment).ToArray());
            int index = metadata.AddressAlignment;
            var result = new List<DecodedFunctionEntry>();
            for (var i = 0; i < count; i++)
            {
                var entryResult = Utils.DecodeFunctionEntry(commands.Skip(index).ToArray(), metadata);

                result.Add(entryResult.Item1);
                index += entryResult.Item2;
            }

            return (result, index);
        }

        public static IEnumerable<DecodedCommand> ParseCommands(IEnumerable<byte> commands, PackageMetadata metadata)
        {
            return CommandSelector.ParseAllCommands(commands, metadata);
        }
    }
}
