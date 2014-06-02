namespace OSW.Common.Utilities
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Extensions;

    public static class ManagedDateTime
    {
        private static DateTime? now;

        public static DateTime ForceNowToBe(DateTime dateTime)
        {
            var names = new[] { Assembly.GetCallingAssembly(), Assembly.GetEntryAssembly() }
                              .Where(x => x != null)
                              .Select(x => x.GetName().Name)
                              .Distinct()
                              .ToList();

            if (!names.Any(x => x.ToLower().Contains("test")))
            {
                var errorMessage = "Attempt to force Date.Now to be {0} made from assembl{1} {2}. This operation can only be called by a test assembly.".FormatWith(dateTime, names.Count == 1 ? "y" : "ies", names.JoinComma());
                throw new Exception(errorMessage);
            }

            now = dateTime;
            return Now;
        }

        public static DateTime Now
        {
            get
            {
                return now ?? TheRealNow;
            }
        }

        public static DateTime TheRealNow
        {
            get
            {
                return DateTime.Now;
            }
        }

        public static DateTime ClearForcedNowValue()
        {
            now = null;
            return Now;
        }
    }
}