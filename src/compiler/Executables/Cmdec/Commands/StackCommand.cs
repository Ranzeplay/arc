using Arc.Cmdec.Models;
using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec.Commands
{
    internal class StackCommand
    {
        public static DecodeResult PushInstant(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var dataBlock = Utils.DecodeDataBlock(commands.Skip(1).ToArray(), metadata);

            var len = 1 + dataBlock.Item2;
            return new(len, new(location, commands.Take(len).ToArray(), "Push an instant value to stack"));
        }

        public static DecodeResult PushFromObject(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var accessor = Utils.DecodeDataAccessor(commands.Skip(1).ToArray(), metadata);

            var len = 1 + accessor.Item2;
            return new(len, new(location, commands.Take(len).ToArray(), "Push an object to stack"));
        }

        public static DecodeResult PushFromConstant(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var len = 1 + metadata.DataSlotAlignment;
            return new(len, new(location, commands.Take(len).ToArray(), "Push a constant to stack"));
        }

        public static DecodeResult Pop(long location, IEnumerable<byte> commands, PackageMetadata _)
        {
            var len = 1;
            return new(len, new(location, commands.Take(len).ToArray(), "Pop the top element from the stack and delete it"));
        }

        public static DecodeResult PopToObject(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var accessor = Utils.DecodeDataAccessor(commands.Skip(1).ToArray(), metadata);

            var len = 1 + accessor.Item2;
            return new(len, new(location, commands.Take(len).ToArray(), "Pop the top element from the stack and save it to an object"));
        }
    }
}
