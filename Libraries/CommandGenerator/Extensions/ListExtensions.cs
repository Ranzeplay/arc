using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    internal static class Extensions
    {
        public static void ReplaceRange<T>(this IList<T> target, IEnumerable<T> values, int beginIndex)
        {
            for (int i = 0; i < values.Count(); i++)
            {
                target[beginIndex + i] = values.ElementAt(i);
            }
        }
    }
}
