using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcIntegerEncoder : IArcConstantEncoder
    {
        public IEnumerable<byte> Encode(object o)
        {
            if (o is long integer)
            {
                return BitConverter.GetBytes(integer);
            }
            else
            {
                throw new ArgumentException("Expected an integer");
            }
        }
    }
}
