using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> property)
        {
            return source.GroupBy(property).Select(x => x.First());
        }

        public static IEnumerable<TSource> TakeWhileAggregate<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func,
            Func<TAccumulate, bool> predicate)
        {
            TAccumulate accumulator = seed;
            foreach (TSource item in source)
            {
                accumulator = func(accumulator, item);
                if (predicate(accumulator))
                {
                    yield return item;
                }
                else
                {
                    yield break;
                }
            }
        }

        public static double Median<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            var array = source.Select(selector).OrderBy(x => x).ToArray();

            if (!array.Any()) 
            {
                return 0;
            }

            var count = array.Count();

            if (count % 2 == 0)
            {
                return (array[count / 2 - 1] + array[count / 2]) / 2;
            } 
            else
            {
                return array[(count - 1) / 2];
           }
        }
    }
}
