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
    }
}
