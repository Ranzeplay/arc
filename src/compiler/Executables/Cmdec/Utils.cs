using Arc.Cmdec.Models;
using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec
{
    internal class Utils
    {
        public static (byte[], int) DecodeDataBlock(byte[] commands, PackageMetadata metadata)
        {
            var size = commands[0];
            var length = metadata.DataSectionSize * size;
            var data = commands.Take(length);

            return (data.ToArray(), length + 1);
        }

        public static (DecodedFunctionEntry, int) DecodeFunctionEntry(byte[] commands, PackageMetadata metadata)
        {
            var idBytes = commands.Take(metadata.AddressAlignment)
                .ToArray();
            var entryAddrBytes = commands
                .Skip(metadata.AddressAlignment)
                .Take(metadata.AddressAlignment)
                .ToArray();

            var id = BytesToLong(idBytes);
            var entryAddr = BytesToLong(entryAddrBytes);

            return (new(id, entryAddr), metadata.AddressAlignment * 2);
        }

        public static long BytesToLong(byte[] bytes)
        {
            long result = 0;
            foreach ( var b in bytes )
            {
                result = result * 0x100 + b;
            }

            return result;
        }

        public static (DecodedDataAccessor, int) DecodeDataAccessor(byte[] commands, PackageMetadata metadata)
        {
            var flag = commands[0] == 0x01;
            var slot = commands.Skip(1).Take(metadata.DataSlotAlignment)
                .ToArray();

            return (new(flag, BytesToLong(slot)), 1 + metadata.DataSlotAlignment);
        }
    }
}
