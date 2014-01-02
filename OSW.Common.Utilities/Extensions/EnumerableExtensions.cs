namespace OSW.Common.Utilities.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Union<T>(this IEnumerable<T> collection, T item)
        {
            return collection.Union(item.ToEnumerable());
        }

        public static bool ContainsAll<T>(this IEnumerable<T> collection, IEnumerable<T> otherCollection)
        {
            return collection.All(otherCollection.Contains);
        }

        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }

        public static T[] AsArray<T>(this T item)
        {
            return item.ToEnumerable().ToArray();
        }

        public static void Each<TEntity>(this IEnumerable<TEntity> collection, Action<TEntity> action)
        {
            foreach (var entity in collection)
            {
                action(entity);
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null) return true;
            return !collection.Any();
        }

        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> collection)
        {
            return collection.NullSafe().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key);
        }

        public static IEnumerable<T> NullSafe<T>(this IEnumerable<T> collection)
        {
            return collection ?? Enumerable.Empty<T>();
        }

        public static bool Contain<T>(this FluentWrapper<IEnumerable<T>> fluentWrapper, T item)
        {
            return fluentWrapper.Evaluate(x => x.Contains(item));
        }

        public static IDictionary<TKey, IList<TItem>> SplitBy<TItem, TKey>(this IEnumerable<TItem> collection, Func<TItem, TKey> keySelector)
        {
            return collection.GroupBy(keySelector).ToDictionary(x => x.Key, x => (IList<TItem>)x.ToList());
        }
    }
}