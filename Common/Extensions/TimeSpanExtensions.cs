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
            else if (t.TotalMinutes <= 1)
            {
                return $@"{t:%s} second(s)";
            }
            else if (t.TotalHours <= 1)
            {
                return $@"{t:%m} minute(s)";
            }
            else if (t.TotalDays <= 1)
            {
                return $@"{t:%h} hour(s)";
            }
            else
                return $@"{t:%d} day(s)";
        }
    }
}
