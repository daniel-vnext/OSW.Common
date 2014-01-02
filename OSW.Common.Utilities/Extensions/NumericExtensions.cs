namespace OSW.Common.Utilities.Extensions
{
    using System;

    using log4net;

    using OSW.Common.Utilities;

    public static class NumericExtensions
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NumericExtensions));

        private const int SignificantFigureToRoundTo = 1000;

        public static decimal Round(this decimal numberToRound)
        {
            var isNegative = numberToRound.Is().Not.Positive();
            numberToRound = numberToRound.Absolute();

            const decimal midpoint = SignificantFigureToRoundTo / 2m;

            if (numberToRound < midpoint)
            {
                return numberToRound.InvertIfNeedsBe(isNegative);
            }

            var amountOverMidpoint = numberToRound % SignificantFigureToRoundTo;
            var lowerLimit = numberToRound - amountOverMidpoint;

            var rounded = lowerLimit + (amountOverMidpoint >= midpoint ? SignificantFigureToRoundTo : 0);

            Log.DebugFormat("{0} rounded to {1}, using significant figure to round to {2}", numberToRound, rounded, SignificantFigureToRoundTo);

            return rounded.InvertIfNeedsBe(isNegative);
        }

        private static decimal InvertIfNeedsBe(this decimal value, bool isNegative)
        {
            return isNegative ? value * -1 : value;
        }

        public static int Hundred(this int input)
        {
            return input * 100;
        }

        public static int Thousand(this int input)
        {
            return input * 1000;
        }

        public static decimal Thousand(this decimal input)
        {
            return input * 1000;
        }

        public static int Million(this int input)
        {
            return input.Thousand().Thousand();
        }

        public static decimal Million(this decimal input)
        {
            return input.Thousand().Thousand();
        }

        public static decimal Invert(this decimal input)
        {
            return input * -1;
        }

        public static bool SameSignAs(this FluentWrapper<decimal> fluentWrapper, decimal other)
        {
            Func<decimal, bool> sameSign = x => x.Is().Zero() || other.Is().Zero() || (x.Is().LessThan(0) && other.Is().LessThan(0)) || (x.Is().GreaterThan(0) && other.Is().GreaterThan(0));

            return fluentWrapper.Evaluate(sameSign);
        }

        public static decimal Absolute(this decimal value)
        {
            return Math.Abs(value);
        }

        public static bool Positive(this FluentWrapper<decimal> fluentWrapper)
        {
            return fluentWrapper.Evaluate(x => x.Is().GreaterThanOrEqualTo(0));
        }

        public static bool Zero(this FluentWrapper<decimal> fluentWrapper)
        {
            return fluentWrapper.Evaluate(x => x.Is().EqualTo(0));
        }

        public static bool Zero(this FluentWrapper<int> fluentWrapper)
        {
            return fluentWrapper.Evaluate(x => ((decimal)x).Is().EqualTo(0));
        }

        public static bool LessThan(this FluentWrapper<decimal> fluentWrapper, decimal other)
        {
            return fluentWrapper.Evaluate(x => x < other);
        }

        public static bool LessThanOrEqualTo(this FluentWrapper<decimal> fluentWrapper, decimal other)
        {
            return fluentWrapper.Evaluate(x => x <= other);
        }

        public static bool GreaterThan(this FluentWrapper<decimal> fluentWrapper, decimal other)
        {
            return fluentWrapper.Evaluate(x => x > other);
        }

        public static bool GreaterThanOrEqualTo(this FluentWrapper<decimal> fluentWrapper, decimal other)
        {
            return fluentWrapper.Evaluate(x => x >= other);
        }

        public static bool EqualTo(this FluentWrapper<decimal> fluentWrapper, decimal other)
        {
            return fluentWrapper.Evaluate(x => x == other);
        }

        public static decimal Percent(this int value)
        {
            return new decimal(value).Percent();
        }

        public static decimal Percent(this decimal value)
        {
            return value / 100m;
        }
    }
}