namespace OSW.Common.Utilities.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;

    public static class DelegateExtensions
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DelegateExtensions));

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
                        Log.Error("Error performaing action", ex);
                        exceptions.Add(ex);
                        Thread.Sleep(wait);
                    }

                    numberOfRetries--;
                }

                var message = "Unable to perform action after {0} retries, with a {1} wait time between. Errors: {2}".FormatWith(numberOfRetries, wait, exceptions.CollectionToString());
                Log.Error(message);
            });
        }
    }
}