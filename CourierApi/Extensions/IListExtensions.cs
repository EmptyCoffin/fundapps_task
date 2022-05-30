using System;
using System.Collections.Generic;

namespace CourierApi.Extensions
{
    public static class IListExtensions
    {
        public static int FindIndex<T>(this IList<T> source, Predicate<T> match)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (match(source[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}