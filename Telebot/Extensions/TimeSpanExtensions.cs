using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToReadable(this TimeSpan t)
        {
            if (t.TotalSeconds <= 1)
            {
                return $@"{t:s\.ff} seconds";
            }
            else if (t.TotalMinutes <= 1)
            {
                return $@"{t:%s} seconds";
            }
            else if (t.TotalHours <= 1)
            {
                return $@"{t:%m} minutes";
            }
            else if (t.TotalDays <= 1)
            {
                return $@"{t:%h} hours";
            }
            else return $@"{t:%d} days";
        }
    }
}
