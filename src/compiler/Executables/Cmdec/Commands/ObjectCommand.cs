using Arc.Cmdec.Models;
using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec.Commands
{
    internal class ObjectCommand
    {
        public static DecodeResult CreateLocal(long location, IEnumerable<byte> commands)
        {
            return new(1, new(location, commands.Take(1).ToArray(), "Create a local object"));
        }

        public static DecodeResult CreateGlobal(long location, IEnumerable<byte> commands)
        {
            return new(1, new(location, commands.Take(1).ToArray(), "Create a global object"));
        }

        public static DecodeResult DeleteLocal(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var len = 1 + metadata.DataSlotAlignment;
            return new(len, new(location, commands.Take(len).ToArray(), "Create a local object"));
        }

        public static DecodeResult DeleteGlobal(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var len = 1 + metadata.DataSlotAlignment;
            return new(len, new(location, commands.Take(len).ToArray(), "Create a global object"));
        }
    }
}
