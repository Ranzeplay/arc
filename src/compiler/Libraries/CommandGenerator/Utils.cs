namespace Arc.Compiler.CommandGenerator
{
    internal class Utils
    {
        internal static byte[] CombineLeadingCommand(params byte[] commands)
        {
            var result = new List<byte>();

            bool flip = false;
            foreach (var b in commands)
            {
                if (flip)
                {
                    result[^1] *= 0x10;
                    result[^1] += b;
                }
                else
                {
                    result.Add(b);
                }

                flip = !flip;
            }

            return result.ToArray();
        }

        internal static byte[] GenerateDataAligned(long data, byte width)
        {
            var result = BitConverter.GetBytes(data).ToArray();
            Array.Resize(ref result, width);
            Array.Reverse(result);

            return result;
        }
    }
}