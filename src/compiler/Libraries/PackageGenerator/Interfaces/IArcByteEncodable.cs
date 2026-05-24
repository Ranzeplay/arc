namespace Arc.Compiler.PackageGenerator.Interfaces;

public interface IArcByteEncodable
{
    public IEnumerable<byte> Encode();
}