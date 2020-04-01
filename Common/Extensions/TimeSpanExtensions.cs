using System;

namespace Common.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToReadable(this TimeSpan t)
        {
            if (t.TotalSeconds <= 1)
            {
                return $@"{t:s\.ff} second(s)";
            }

            if (t.TotalMinutes <= 1)
            {
                return $@"{t:%s} second(s)";
            }

            if (t.TotalHours <= 1)
            {
                return $@"{t:%m} minute(s)";
            }

            if (t.TotalDays <= 1)
            {
                return $@"{t:%h} hour(s)";
            }

            return $@"{t:%d} day(s)";
        }
    }
}
