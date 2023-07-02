using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec.Commands
{
    internal class JumpCommand
    {
        public static DecodeResult Relative(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var len = 1 + metadata.AddressAlignment;
            return new(len, new(location, commands.Take(len).ToArray(), "Jump to relative address"));
        }

        public static DecodeResult Conditional(long location, IEnumerable<byte> commands, PackageMetadata metadata)
        {
            var len = 1 + 2 * metadata.AddressAlignment;
            return new(len, new(location, commands.Take(len).ToArray(), "Jump to different address by judging stack top value"));
        }
    }
}
