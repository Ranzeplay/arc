using Arc.Compiler.PackageGenerator.Interfaces;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcDecimalEncoder : IArcConstantEncoder
    {
        public IEnumerable<byte> Encode(object o)
        {
            if (o is decimal dec)
            {
                return BitConverter.GetBytes((double)dec);
            }
            else
            {
                throw new ArgumentException("Expected a decimal");
            }
        }
    }
}
