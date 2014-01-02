namespace OSW.Common.Utilities.Extensions
{
    using System.Collections.Generic;

    using OSW.Common.Utilities;

    public static class FluentExtensions
    {
        public static FluentWrapper<T> Is<T>(this T value)
        {
            return new FluentWrapper<T>(value);
        }

        public static FluentWrapper<IEnumerable<T>> Does<T>(this IEnumerable<T> collection)
        {
            return new FluentWrapper<IEnumerable<T>>(collection.NullSafe());
        }
    }
}