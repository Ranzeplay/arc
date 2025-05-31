namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal class ArcArrayEncoder
    {
        public static IEnumerable<byte> SerializeArray(IEnumerable<long> array)
        {
            return [
                ..BitConverter.GetBytes((long)array.Count()),
                ..array.Select(BitConverter.GetBytes).SelectMany(x => x)
                ];
        }
    }
}
