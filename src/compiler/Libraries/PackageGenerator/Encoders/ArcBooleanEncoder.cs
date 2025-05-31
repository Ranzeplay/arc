using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcBooleanEncoder : IArcConstantEncoder
    {
        public IEnumerable<byte> Encode(object o)
        {
            if (o is bool boolean)
            {
                return BitConverter.GetBytes(boolean);
            }
            else
            {
                throw new ArgumentException("Expected a boolean");
            }
        }
    }
}
