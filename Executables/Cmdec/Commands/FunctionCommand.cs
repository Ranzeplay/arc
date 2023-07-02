using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec.Commands
{
    internal class FunctionCommand
    {
        public static DecodeResult Enter(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var len = 1 + metadata.AddressAlignment;
            return new(len, new(location, commands.Take(len).ToArray(), "Enter function"));
        }

        public static DecodeResult LeaveWithValue(long location, IEnumerable<byte> commands)
        {
            var len = 1;
            return new(len, new(location, commands.Take(len).ToArray(), "Leave function with value"));
        }

        public static DecodeResult LeaveWithoutValue(long location, IEnumerable<byte> commands)
        {
            var len = 1;
            return new(len, new(location, commands.Take(len).ToArray(), "Leave function without value"));
        }
    }
}
