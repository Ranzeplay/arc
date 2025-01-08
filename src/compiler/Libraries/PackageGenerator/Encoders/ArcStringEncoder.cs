using Arc.Compiler.PackageGenerator.Interfaces;
using System.Text;

namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcStringEncoder : IArcConstantEncoder
    {
        public IEnumerable<byte> Encode(object o)
        {
            if (o is string str)
            {
                return [..BitConverter.GetBytes((long)str.Length), ..Encoding.UTF8.GetBytes(str)];
            }
            else
            {
                throw new ArgumentException("Expected a string");
            }
        }
    }
}
