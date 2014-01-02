namespace OSW.Common.Utilities.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using OSW.Common.Utilities;

    public static class TypeExtensions
    {
        public static string GetPropertyName<TType, TProperty>(this TType subject, Expression<Func<TType, TProperty>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            return memberExpression == null ? string.Empty : memberExpression.Member.Name;
        }

        public static TEnum ParseEnum<TEnum>(this string value, bool ignoreCase) where TEnum : struct
        {
            TEnum parsedEnumValue;

            value = value.Replace(" ", string.Empty);

            if (!Enum.TryParse(value, ignoreCase, out parsedEnumValue)) throw new ArgumentException("Cannot find value for {0} in enum type {1}".FormatWith(value, typeof(TEnum).FullName));

            return parsedEnumValue;
        }

        public static T ParseEnum<T>(this string value) where T : struct
        {
            return value.ParseEnum<T>(false);
        }

        public static TEnumDest MapEnum<TEnumSource, TEnumDest>(this TEnumSource source) where TEnumDest : struct where TEnumSource : struct
        {
            return source.ToString().ParseEnum<TEnumDest>();
        }

        public static IEnumerable<T> GetPublicFieldValuesOfType<T>(this Type type)
        {
            var allFields = GetPublicFields(type);
            var desiredFieldType = typeof(T);
            var fieldsOfMatchingType = allFields.Where(x => x.FieldType == desiredFieldType);
            return fieldsOfMatchingType.Select(fieldInfo => (T)fieldInfo.GetValue(null));
        }

        public static IEnumerable<FieldInfo> GetPublicFields(this Type type)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public);
        }

        public static int ToIntWithDefault(this string value)
        {
            int retVal;
            int.TryParse(value, out retVal);
            return retVal;
        }

        public static decimal ToDecimal(this double value)
        {
            return Convert.ToDecimal(value);
        }

        public static decimal ToDecimalWithDefault<T>(this T? value) where T : struct
        {
            return !value.HasValue ? 0m : Convert.ToDecimal(value);
        }

        public static string Print(this object obj)
        {
            if (obj == null)
            {
                return "<NULL>";
            }
            if (obj == DBNull.Value)
            {
                return "<DBNull>";
            }

            return obj.ToString();
        }

        public static TResult Reach<TRoot, TResult>(this TRoot root, Func<TRoot, TResult> accessor) where TRoot : class
        {
            return root == null ? default(TResult) : accessor(root);
        }

        public static bool DefaultValue<T>(this FluentWrapper<T> item)
        {
            Func<T, bool> predicate = x =>
                {
                    var @default = default(T);
                    return x == null || x.Equals(@default);
                };

            return item.Evaluate(predicate);
        }
    }
}