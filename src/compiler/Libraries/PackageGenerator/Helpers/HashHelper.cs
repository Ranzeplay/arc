namespace Arc.Compiler.PackageGenerator.Helpers
{
    static class HashHelper
    {
        public static ulong CalculateHash(string read)
        {
            var hashedValue = 3074457345618258791ul;
            for (int i = 0; i < read.Length; i++)
            {
                hashedValue += read[i];
                hashedValue *= 3074457345618258799ul;
            }
            return hashedValue;
        }
        
        public static uint GetDjb2Hash(ReadOnlySpan<char> input)
        {
            uint hash = 5381;

            for (int i = 0; i < input.Length; i++)
            {
                hash = ((hash << 5) + hash) + input[i];
            }

            return hash;
        }
    }
}
