namespace Arc.Compiler.PackageGenerator.Encoders
{
    internal static class ArcArrayEncoder
    {
        public static IEnumerable<byte> SerializeArray(IEnumerable<long> array)
        {
            return [
                ..BitConverter.GetBytes((long)array.Count()),
                ..array.Select(BitConverter.GetBytes).SelectMany(x => x)
                ];
        }
        
        public static IEnumerable<byte> SerializeArray(IEnumerable<ulong> array)
        {
            return [
                ..BitConverter.GetBytes(array.LongCount()),
                ..array.Select(BitConverter.GetBytes).SelectMany(x => x)
            ];
        }
    }
}
