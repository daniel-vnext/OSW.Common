namespace OSW.Common.Utilities.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using log4net;

    public static class StringExtensions
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StringExtensions));

        public static string JoinWith<T>(this IEnumerable<T> values, string separator)
        {
            return string.Join(separator, values.NullSafe());
        }

        public static string JoinComma<T>(this IEnumerable<T> values)
        {
            return values.JoinWith(", ");
        }

        public static string JoinSemiColon<T>(this IEnumerable<T> values)
        {
            return values.JoinWith("; ");
        }

        public static string JoinNewLine<T>(this IEnumerable<T> values)
        {
            return values.JoinWith(Environment.NewLine);
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args.ToArray());
        }

        public static string Remove(this string input, string toRemove)
        {
            return input.Replace(toRemove, string.Empty);
        }

        public static bool HasValue(this string input)
        {
            return !input.IsNullOrEmpty();
        }

        public static IEnumerable<string> SplitBy(this string input, string separator, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            return input.Split(separator.ToEnumerable().ToArray(), stringSplitOptions);
        }

        public static bool NullOrEmpty(this FluentWrapper<string> fluentWrapper)
        {
            return fluentWrapper.Evaluate(string.IsNullOrEmpty);
        }

        public static int ToInteger(this string input)
        {
            var elements = input.SplitBy(" ").ToList();
            var value = Convert.ToDecimal(elements.First().Replace(",", string.Empty));

            if (elements.Count == 2 && elements[1].ToLowerInvariant() == "million")
            {
                value *= 1.Million();
            }

            return (int)value;
        }

        public static bool ToBoolean(this string input)
        {
            return Convert.ToBoolean(input);
        }

        public static decimal ToDecimal(this string input)
        {
            if (input == null) throw new ArgumentException("Cannot convert empty value to a Decimal");

            string s = input.Replace("(", String.Empty).Replace(")", String.Empty);

            if (String.IsNullOrEmpty(s)) throw new ArgumentException("Cannot convert Empty String to Decimal");

            if (s == "-") return 0M;

            bool percent = false;
            if (s.EndsWith("%"))
            {
                s = s.Substring(0, s.Length - 1);
                percent = true;
            }

            decimal result;

            // detect scientific notation, and convert to double
            if (s.Contains("E") || s.Contains("e"))
            {
                try
                {
                    result = (decimal)double.Parse(s);
                }
                catch (FormatException)
                {
                    throw new FormatException("Couldn't convert value '{0}' to a Decimal".FormatWith(s));
                }
            }
            else
            {
                if (!decimal.TryParse(s, out result)) throw new FormatException("Couldn't convert value '{0}' to a Decimal".FormatWith(s));
            }

            return percent ? result / 100M : result;
        }

        public static IEnumerable<string> SplitBy(this string value, string delimiter)
        {
            return value.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string CollectionToString<T>(this IEnumerable<T> collection)
        {
            return "{{ {0} }}".FormatWith(collection.NullSafe().JoinWith(", "));
        }

        public static Guid ToGuid(this string input)
        {
            return Guid.Parse(input);
        }

        public static string GetUniqueTermFor(this IEnumerable<string> current, string target, int maxLength = int.MaxValue, int numberOfAttempts = int.MaxValue)
        {
            Log.DebugFormat("Getting unique term for {0}", target);

            var originalTarget = target;
            numberOfAttempts = numberOfAttempts - 1;

            current = current.NullSafe().ToList();
            target = new string(originalTarget.Take(maxLength).ToArray());

            if (!current.Contains(target))
            {
                return target;
            }

            foreach (var suffix in Enumerable.Range(1, numberOfAttempts).Select(i => " ({0})".FormatWith(i)))
            {
                target = new string(originalTarget.Take(maxLength - suffix.Length).ToArray()) + suffix;
                if (!current.Contains(target))
                {
                    return target;
                }
            }

            throw new InvalidOperationException("Unable to get next available term for '{0}'".FormatWith(originalTarget));
        }

        public static T[] ToArrayOf<T>(this string input, string delimiter = ", ")
        {
            Func<string, object> convert;
            var type = typeof(T);

            if (type == typeof(int)) convert = x => x.ToInteger();
            else if (type == typeof(bool)) convert = x => x.ToBoolean();
            else if (type == typeof(string)) convert = x => x;
            else throw new Exception("Case {0} not handled".FormatWith(type));

            return (input ?? string.Empty).Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(x => (T)convert(x)).ToArray();
        }
    }
}