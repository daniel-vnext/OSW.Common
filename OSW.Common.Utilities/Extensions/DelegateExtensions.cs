namespace OSW.Common.Utilities.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public static class DelegateExtensions
    {
        public static Func<T, object> WrapInEmptyFunc<T>(this Action<T> action)
        {
            Func<T, object> func = parameter =>
            {
                action(parameter);
                return null;
            };

            return func;
        }

        public static void PerformAsyncAndFailSilently(this Action action, int numberOfRetries = 3, TimeSpan wait = default(TimeSpan))
        {
            var taskFactory = new TaskFactory();
            taskFactory.StartNew(() =>
            {
                if (wait == default(TimeSpan))
                {
                    wait = TimeSpan.FromSeconds(1);
                }

                var exceptions = new List<Exception>();

                while (numberOfRetries > 0)
                {
                    try
                    {
                        action();
                        return;
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        Thread.Sleep(wait);
                    }

                    numberOfRetries--;
                }
            });
        }
    }
}