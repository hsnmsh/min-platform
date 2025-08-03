namespace MinPlatform.Data.Service.Extensions
{
    using System.Collections.Generic;

    public static class ListExtensions
    {
        public static bool IsLastItem<T>(this IList<T> list, T item)
        {
            return list.IndexOf(item) == list.Count - 1;
        }
    }
}
