namespace Arc.Compiler.PackageGenerator.Helpers
{
    internal static class ListHelper
    {
        public static void ReplaceRange<T>(this IList<T> list, int index, IEnumerable<T> collection)
        {
            for (int i = 0; i < collection.Count(); i++)
            {
                list[index + i] = collection.ElementAt(i);
            }
        }
    }
}
