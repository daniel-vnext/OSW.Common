namespace OSW.Common.Utilities.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumExtensions
    {
        public static TTarget? MapToNullable<TSource, TTarget>(this TSource? sourceEnumValue)
            where TSource : struct
            where TTarget : struct
        {
            return !sourceEnumValue.HasValue ? (TTarget?)null : sourceEnumValue.Value.MapTo<TSource, TTarget>();
        }

        public static TTarget MapTo<TSource, TTarget>(this TSource sourceEnumValue)
            where TSource : struct
            where TTarget : struct
        {
            return (TTarget)Enum.Parse(typeof(TTarget), Enum.GetName((typeof(TSource)), sourceEnumValue), true);
        }

        public static IEnumerable<T> GetValues<T>(this Type enumType) where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T Parse<T>(this string input) where T : struct
        {
            var type = typeof(T);
            var values = GetValues<T>(type).Where(x => x.ToString() == input).ToList();

            if (!values.Any())
            {
                throw new ArgumentException("Cannot find enum value '{0}' for type {1}".FormatWith(input, type.Name));
            }

            return values.Single();
        }
    }
}