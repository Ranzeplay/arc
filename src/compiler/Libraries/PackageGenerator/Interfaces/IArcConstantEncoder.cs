namespace Arc.Compiler.PackageGenerator.Interfaces
{
    public interface IArcConstantEncoder
    {
        IEnumerable<byte> Encode(object o);
    }
}
