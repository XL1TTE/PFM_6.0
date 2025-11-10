using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Extentions
{
    public static class ListExtentions
    {

        private static Random rng = new Random();

        public static T PopLast<T>(this List<T> list)
        {
            var value = list.Last();
            list.RemoveAt(list.Count - 1);
            return value;
        }

        public static T PopFirst<T>(this List<T> list)
        {
            var value = list.First();
            list.RemoveAt(0);
            return value;
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}
